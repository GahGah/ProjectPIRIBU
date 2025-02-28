﻿using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;
using UnityEngine;

public class UnitCharacter : Unit
{
	[SerializeField] protected LayerMask groundLayer;
	[HideInInspector] public Character character;
	[HideInInspector] public List<Collider2D> IgnoreColliders;//무시하는 지형들
	[HideInInspector] public bool isGrounded = false;
	public CharacterFoot foot;
	public CapsuleCollider2D body;
	public StageObjectSensor sensor;
	public CharacterStatus status;

	//입력에 맞추어 좌우 이동방향 선택
	public int GetSideMoveDirection() {
		int moveDir = 0;
		if (InputManager.Instance.buttonLeft.isPressed) moveDir = -1;
		if (InputManager.Instance.buttonRight.isPressed) moveDir = 1;
		return moveDir;
	}

	protected override void Awake() {
		base.Awake();
		IgnoreColliders = new List<Collider2D>();
	}

	//이동속도 처리 함수 (bool값으로 벽이 앞에있는지 반환)
	public bool HandleMoveSpeed(int moveDir, MoveSpeed moveSpeed, bool wallCheck = true) {
		float speed = status.sideMoveSpeed;
		int speedDir = (speed > 0 ? 1 : -1);

		//벽 충돌체크
		bool isWallForward = false;
		if (wallCheck) {
			if (WallCheck(moveDir)) {
				isWallForward = true;
				moveDir = 0;
				//브레이크
				if (speed * speedDir < moveSpeed.brake)
					speed = 0;
				else
					speed -= speedDir * moveSpeed.brake;
			}
		}

		if (moveDir != 0) {
			//가속
			speed += moveDir * moveSpeed.accel;
		} else {
			//브레이크
			if (speed * speedDir < moveSpeed.brake)
				speed = 0;
			else
				speed -= speedDir * moveSpeed.brake;
		}
		//최대속력 제한
		if (speed * speedDir > moveSpeed.max) {
			speed = speedDir * moveSpeed.max;
		}

		status.sideMoveSpeed = speed;
		if (moveDir != 0) status.modelSide = moveDir;

		return isWallForward;
	}

	//OneWay 플랫폼 관련 충돌처리 및 Parenting
	public void ManageColliders() {
		Vector3 footPos = foot.transform.position;

		//플랫폼 순회
		foreach (LinearPlatform platform in sensor.linearPlatforms) {
			Collider2D targetColl = platform.coll;
			// One-way 플랫폼일 경우
			if (platform.isOneWay) {
				bool isIgnore = false;
				//부모는 밟고있어야 한다.
				if (platform == liftParent) {
					foot.IgnoreWith(targetColl, isIgnore);
					IgnoreColliders.Remove(targetColl);
					continue;
				} else {
					isIgnore = platform.GetIsOneWayIgnore(footPos);
					// One-Way 플랫폼 충돌무시 검사
					foot.IgnoreWith(targetColl, isIgnore);
					if (isIgnore) {
						if (!IgnoreColliders.Contains(targetColl))
							IgnoreColliders.Add(targetColl);
					} else IgnoreColliders.Remove(targetColl);
				}
			}
		}

		//Lifting Platform을 Parent로 (땅에 붙어있는 상태에서만)
		LiftObject lift = null;
		if (foot.adjacentLiftObjects.Count > 0 && isGrounded) {
			lift = foot.adjacentLiftObjects[0];
		}
		SetLiftParent(lift);
		//if (parent) parent.Draw();
	}

	#region Ground Detecting 관련

	[HideInInspector] public RaycastHit2D raycastHitGround;
	[HideInInspector] public Vector2 groundForward;
	[HideInInspector] public float groundDist = 0.2f;//최소 지형 이격거리
	[HideInInspector] public float groundDegree = 50;//지형으로 판정되는 각도

	//해당 면의 노말이 땅인가?
	public bool IsGroundNormal(Vector2 normal) {
		return Vector2.Angle(transform.up, normal) <= groundDegree ? true : false;
	}

	//캐릭터를 땅에 붙임. groundForward가 업데이트됨. 붙이지 못했을시 false 반환
	//Logic Error : Attach 과정 자체가 캐릭터를 내리는 행위라 mass가 더 붙은 듯한 문제가 있음.
	public bool AttachGround() {
		float rayDist;
		Vector2 groundNormal;

		//처음엔 아래방향으로 RayCast하여 지면 노말을 검사
		rayDist = RayGroundFromFoot(Vector2.down);
		groundNormal = raycastHitGround.normal;
		groundForward = new Vector2(groundNormal.y, -groundNormal.x);
		//발과 지형간의 최소거리인 rayDist값으로 지형부착 이동을 한다.
		rayDist = RayGroundFromFoot(-groundNormal);
		groundNormal = raycastHitGround.normal;
		groundForward = new Vector2(groundNormal.y, -groundNormal.x);

		//지형부착 성공
		if (groundDist >= rayDist && IsGroundNormal(groundNormal)) {
			//Parent에 아래힘 가하기
			if (liftParent)
				liftParent.SetMovement(
					MovementType.ForceAt,
					Vector2.down * rigid.mass * status.fallSpeed * fixedUpdatePerSec,
					foot.transform.position
				);

			if (rayDist > 0) {//일반적인 부착값
				rayDist += 0.02f;
			} else {
			//Dist가 음수면 foot가 올라가게 되어있다.
				//모든 RayHit가 발 위라면 플랫폼에 끼였을 가능성이 높다.
				if (isAllRayOverFoot)
					rayDist *= 0.5f;
				else
					rayDist *= 0.2f;
			}
			SetMovement(MovementType.AddPos, -groundNormal * rayDist);
			isGrounded = true;
			return true;
		}

		return false;
	}

	bool isAllRayOverFoot;//지형검사에서 모든 Ray가 원점보다 위였는가?
	float rayGroundOffset = 0.25f;//발보다 약간 위에서 쬐어주는 값
	/// <summary>
	/// 지형에 RayCast하고 거리가 가장 짧은 ray를 raycastHitGround에 저장 및 가장 짧은 거리를 반환
	/// </summary>
	public float RayGroundFromFoot(Vector2 _rayDir, bool isSetRayHit = true) {
		float dist = Mathf.Infinity;
		isAllRayOverFoot = true;

		//여러군데 검사
		int rays = 2;
		for (int i = -rays; i <= rays; i ++) {

			Vector2 origin = foot.transform.position
				- foot.transform.up * foot.size.y * 0.5f
				+ foot.transform.right * foot.size.x * ((float)i / rays * 0.5f)
				- (Vector3)_rayDir* rayGroundOffset;


			float distNow = (groundDist + rayGroundOffset) * 2;
			RaycastHit2D hit = Physics2D.Raycast(origin, _rayDir, distNow, groundLayer);

			if (hit.collider) {

				//처음부터 어느 collider에 바로 닿은 hit의 normal은 그냥 -_raydir임
				if (hit.distance == 0) {
					//원웨이 플랫폼에 낀거면 통상적인 지형 노말로 바꿔줌
					if (hit.collider.CompareTag("LinearPlatform")) {
						hit.normal = Vector2.up;
					} else {
						//원웨이 플랫폼도 아니라면 벽일 수 있음. offset 없이 진짜 원점에서 출발한 ray로 normal을 탐지함.
						RaycastHit2D subHit = Physics2D.Raycast(origin+ _rayDir * rayGroundOffset, -_rayDir, rayGroundOffset, groundLayer);
						if (subHit.distance != 0) {
							hit.normal = subHit.normal;
						} else {
							//이때도 충돌체에 끼어서 distance가 0이 나오면 그냥 원웨이 땅에 끼었을 확률 높음
							//이미 원웨이 플랫폼인지 검사했지만, 보험용으로 한번 더 처리
							hit.normal = Vector2.up;
						}
					}
				}

				if (IsGroundNormal(hit.normal)) {
					//논리적으로 우항은 0이어야 하지만 약간 완화
					if (hit.distance - rayGroundOffset > -rayGroundOffset * 0.5f)
						isAllRayOverFoot = false;

					distNow = hit.distance - rayGroundOffset;
					//dist값 업데이트, 지형 노말인 Ray만 groundRay로 쳐줌
					if (distNow < dist) {
						dist = distNow;
						if (isSetRayHit) raycastHitGround = hit;
					}
				}
			} else {
				isAllRayOverFoot = false;
			}
			
			/*
			Color color;
			if (distNow < 0)
				color = Color.blue;
			else color = Color.red;
			Debug.DrawLine(origin, origin +_rayDir * (distNow+ rayGroundOffset), color);
			Debug.DrawLine(hit.point, hit.point + hit.normal*0.5f, Color.green);
			*/


		}


		return dist;
	}

	/// <summary>
	/// 발 곳곳에서 RayCastAll한 Hit들을 반환
	/// </summary>
	public List<RaycastHit2D> GetRayHitsFromFoot(Vector2 _rayDir) {
		isAllRayOverFoot = true;
		List<RaycastHit2D> ret = new List<RaycastHit2D>();

		//여러군데 검사
		int rays = 2;
		for (int i = -rays; i <= rays; i++) {

			Vector2 origin = foot.transform.position
				- foot.transform.up * foot.size.y * 0.5f
				+ foot.transform.right * foot.size.x * ((float)i / rays * 0.5f)
				- (Vector3)_rayDir * rayGroundOffset;


			RaycastHit2D[] hits = Physics2D.RaycastAll(origin, _rayDir, (groundDist + rayGroundOffset) * 2, groundLayer);
			foreach (RaycastHit2D hit in hits) {
				ret.Add(hit);
			}
		}

		return ret;
	}


	/// <summary>
	/// 공중에서 밟을땅이 있는지 검사
	/// </summary>
	/// <returns>착지판정하는가?</returns>
	public bool GroundCheckFromAir() {

		//착지판정 
		Vector2 _rayDir = Vector2.down;

		isAllRayOverFoot = true;

		//여러군데 검사
		int rays = 2;
		for (int i = -rays; i <= rays; i++) {

			Vector2 origin = foot.transform.position
				- foot.transform.up * foot.size.y * 0.5f
				+ foot.transform.right * foot.size.x * ((float)i / rays * 0.5f)
				- (Vector3)_rayDir * rayGroundOffset;

			RaycastHit2D hit = Physics2D.Raycast(origin, _rayDir, (groundDist + rayGroundOffset) * 2, groundLayer);

			if (hit.collider) {
				//한 ray라도 발 아래에 쬐어지면
				if (hit.distance - rayGroundOffset > -rayGroundOffset * 0.5f)//논리적으로 우항은 0이어야 하지만 약간 완화
					isAllRayOverFoot = false;

				//dist값 업데이트
				float groundYSpeed = 0;
				if (hit.rigidbody)
					groundYSpeed = hit.rigidbody.velocity.y;

				float distNow = hit.distance - rayGroundOffset;
				if (distNow <= 0.05f //땅에 가까이 붙었을때
					&& status.verticalSpeed - groundYSpeed <= 0//추락할때만 땅에 붙게 (지형 속도도 고려)
					&& IsGroundNormal(hit.normal)) {//지형 각도 범위일때

					isGrounded = true;
					return true;
				}
			}

		}

		//땅 못밟으면 이동호출
		Vector2 vel = new Vector2(status.sideMoveSpeed, status.verticalSpeed);
		SetMovement(MovementType.SetVelocity, vel);

		isGrounded = false;
		return false;

	}
	

	/// <summary>
	/// 해당 방향으로 절벽까지의 거리 검사
	/// </summary>
	/// <param name="moveDir"></param>
	/// <returns></returns>
	public float GetWalkableDistance(int moveDir = 1, float maxDistance = 4f, float allowedSpace = -1) {
		float dist = 0;
		if (moveDir == 0) return dist;
		moveDir = moveDir > 0 ? 1 : -1;

		float xStep = 0.08f;

		float startX = transform.position.x;
		float distX = 0;
		float currY = transform.position.y;

		float airSpace = 0;//판별되는 공중간격
		//허용되는 최대 공중간격. 이 크기 이내의 허공은 절벽으로 인식하지 않음.
		if (allowedSpace < 0) { allowedSpace = foot.size.x * 0.8f; }


		//캐릭터 중앙점에서 발 밑까지의 거리 (회전 고려)
		float verticalDist = Vector3.Magnitude(transform.position - (foot.transform.position + -transform.up *foot.size.y * 0.5f));

		for (; distX * moveDir < maxDistance; distX += xStep * moveDir) {
			Vector2 origin = new Vector2(startX + distX, currY);
			RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, verticalDist + groundDist * 2, groundLayer);
			float distNow = hit.distance;
			if (hit.collider && distNow <= verticalDist+groundDist) {
				//Debug.DrawLine(new Vector3(startX+distX, currY), hit.point, Color.magenta);
				currY = hit.point.y + verticalDist;
				airSpace = 0;
			} else {
				//닿은게 아예 없단 것은 공중이란 것
				//Debug.DrawLine(new Vector3(startX + distX, currY), new Vector3(startX + distX, currY-verticalDist), Color.cyan);
				airSpace += xStep;
				if (airSpace >= allowedSpace) {
					break;
				}
			}
		}

		dist = distX*moveDir;
		return dist;
	}
	#endregion

	#region Wall Detecting 관련

	//해당 면의 노말이 벽인가?
	public bool IsWallNormal(Vector2 normal) {
		return Vector2.Angle(transform.up, normal) > groundDegree ? true : false;
	}

	//findTarget이 없는 기본형 : 앞에 벽(GroundLayer와 동일)이 있는지 검사
	//findTarget이 존재 : Target과 마주하고있는지 검사 (PushBox용)
	public bool WallCheck(int dir = 1, GameObject findTarget = null, float maxWallDist = 0.1f) {
		Vector2 pos, size, up, right, rayDir;
		pos = body.transform.position;
		size = body.size;
		right = body.transform.right;
		up = body.transform.up;
		rayDir = right * dir;

		float offsetSide = 0.1f;
		float dist = Mathf.Infinity;

		//여러군데 검사
		float iAdd = 0.2f;//1.0f/5.0f;
		
		for (float i = -0.3f; i <= 0.5f; i += iAdd) {
			Vector2 origin = pos
				+ right * dir * (size.x * 0.5f - offsetSide)
				+ up * (size.y * i);

			RaycastHit2D[] hits = Physics2D.RaycastAll(origin, rayDir, maxWallDist + offsetSide, groundLayer);
			for (int j = 0; j < hits.Length; j++) {
				RaycastHit2D hit = hits[j];
				//Ingore중인 One-Way 플랫폼은 무시
				if (IgnoreColliders.Contains(hit.collider)) continue;

				if (IsWallNormal(hit.normal)) {
					//TargetFind모드.
					if (findTarget == hit.transform.gameObject)
						return true;
					float thisDist = Mathf.Min(dist, hit.distance - offsetSide);
					if (thisDist < dist) {
						dist = thisDist;
					}
					//Debug.DrawLine(origin, hit.point, Color.green);
				}
			}
		}
		//TargetFind 모드가 아니면서, 벽이 감지되었다면
		if (!findTarget && dist < maxWallDist) {
			return true;
		}
		//Target을 발견하지 못했거나, 벽이 감지되지 않았다면
		return false;
	}

	#endregion

	#region 상호작용 오브젝트 관련
	[HideInInspector] public InteractionObject interactionObject;

	//사다리 안에 있는지 판별
	public bool IsInLadder(Vector3 currPos, float offset = 0, InteractionObject ladder = null) {
		if (ladder == null) ladder = interactionObject;
		if (ladder == null) return false;

		Vector2 ladderBottom, ladderTop;
		//offset만큼 양쪽 범위 늘리기
		ladderBottom = ladder.transform.position - ladder.transform.up * (ladder.size.y * 0.5f + offset);
		ladderTop = ladderBottom + (Vector2)ladder.transform.up * (ladder.size.y + offset*2);
		if (Vector2.Dot(ladderBottom - (Vector2)currPos, ladderTop - (Vector2)currPos) <= 0) {
			return true;
		}
		return false;
	}

	//사다리까지의 투영거리 구하기
	public float GetDistanceToLadder(InteractionObject ladder = null) {
		float ret = Mathf.Infinity;
		if (ladder == null) ladder = interactionObject;
		if (ladder == null) return ret;

		ret = Vector3.Magnitude(GetDirectionToLadder(ladder));
		return ret;
	}

	//투영벡터로 캐릭터 중점에서 사다리까지 가는 벡터 구하기
	public Vector2 GetDirectionToLadder(InteractionObject ladder = null) {
		Vector2 ret = Vector2.zero;
		if (ladder == null) ladder = interactionObject;
		if (ladder == null) return ret;

		Vector2 ladderBottom, ladderTop;
		ladderBottom = ladder.transform.position - ladder.transform.up * ladder.size.y * 0.5f;
		ladderTop = ladderBottom + (Vector2)ladder.transform.up * ladder.size.y;
		//Debug.DrawLine(ladderBottom, ladderTop, Color.red);

		Vector2 targetPos = GetProjectionPoint(ladderBottom, ladderTop, transform.position);
		//Debug.DrawLine(transform.position, targetPos, Color.red);

		ret = targetPos - (Vector2)transform.position;
		return ret;
	}
	#endregion

	//투영벡터
	public Vector3 GetProjectionPoint(Vector3 Start, Vector3 End, Vector3 Point) {
		Vector3 dir = Vector3.Normalize(End - Start);
		Vector3 ret = Start + dir * Vector3.Dot(Point - Start, dir);
		return ret;
	}

	//대상 위치가 센서안에 있는가?
	public bool IsInSensor(Vector2 target) {
		if (Vector2.Distance(target,transform.position) <= sensor.radious) {
			return true;
		}
		return false;
	}
}
