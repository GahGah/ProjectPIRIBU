using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class UnitCharacter : Unit
{
	[SerializeField] protected LayerMask groundLayer;
	[HideInInspector] public Character character;
	[HideInInspector] public List<Collider2D> IgnoreColliders;//무시하는 지형들
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

	public override void Awake() {
		base.Awake();
		IgnoreColliders = new List<Collider2D>();
	}

	//이동속도 처리 함수
	public void HandleMoveSpeed(int moveDir, MoveSpeed moveSpeed, bool wallCheck = true) {
		float speed = status.sideMoveSpeed;
		int speedDir = (speed > 0 ? 1 : -1);
		
		//벽 충돌체크
		if (wallCheck) {
			if (WallCheck(moveDir)) {
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
				if (platform == parent) {
					foot.IgnoreWith(targetColl, isIgnore);
					IgnoreColliders.Remove(targetColl);
					continue;
				} else {
					isIgnore = platform.GetIsOneWayIgnore(footPos);
					// One-Way 플랫폼 충돌무시 검사
					foot.IgnoreWith(targetColl, isIgnore);
					if (isIgnore) IgnoreColliders.Add(targetColl);
					else IgnoreColliders.Remove(targetColl);
				}
			}
		}

		//Lifting Platform을 Parent로
		LiftObject lift = null;
		if (foot.adjacentLiftObjects.Count > 0) {
			lift = foot.adjacentLiftObjects[0];
		}
		SetLiftParent(lift);
		//if (parent) parent.Draw();
	}

	[HideInInspector] public float groundDegree = 50;//지형으로 판정되는 각도
	#region Ground Detecting 관련

	[HideInInspector] public RaycastHit2D raycastHitGround;
	[HideInInspector] public Vector2 groundForward;
	[HideInInspector] public float groundDist = 0.5f;//최소 지형 이격거리

	//해당 면의 노말이 땅인가?
	public bool IsGroundNormal(Vector2 normal) {
		return Vector2.Angle(transform.up, normal) <= groundDegree ? true : false;
	}

	//캐릭터를 땅에 붙임. groundForward가 업데이트됨. 붙이지 못했을시 false 반환
	//Logic Error : Attach 과정 자체가 캐릭터를 내리는 행위라 mass가 더 붙은 듯한 문제가 있음.
	public bool AttachGround() {
		RayGround(Vector2.down);
		Vector2 groundNormal = raycastHitGround.normal;
		groundForward = new Vector2(groundNormal.y, -groundNormal.x);

		//발과 지형간의 최소거리인 rayDist값으로 지형부착 이동을 한다.
		float rayDist = RayGround(-groundNormal);
		
		//지형부착 성공
		if (groundDist > rayDist) {
			if (rayDist > 0) //일반적인 부착값
				rayDist += 0.05f;
			else {//Dist가 음수면 foot가 올라가게 되어있다.
				//모든 RayHit가 발 위라면 플랫폼에 끼였을 가능성이 높다.
				if (isAllRayOverFoot)
					rayDist *= 0.2f;
				else
					rayDist *= 0.1f;
			}
			SetMovement(MovementType.AddPos, -groundNormal * rayDist);
			return true;
		}

		//지형부착 실패
		return false;
	}

	bool isAllRayOverFoot;//지형검사에서 모든 Ray가 원점보다 위였는가?
	float rayGroundOffset = 0.25f;//발보다 약간 위에서 쬐어주는 값
	/// <summary>
	/// 지형에 RayCast하고 거리가 가장 짧은 ray를 raycastHitGround에 저장 및 가장 짧은 거리를 반환
	/// </summary>
	public float RayGround(Vector2 _rayDir) {
		float dist = Mathf.Infinity;
		isAllRayOverFoot = true;

		//여러군데 검사
		int rays = 1;
		for (int i = -rays; i <= rays; i ++) {
			Vector2 origin = foot.transform.position
				- foot.transform.up * foot.size.y * 0.5f
				+ foot.transform.right * foot.size.x * ((float)i / rays * 0.5f)
				-(Vector3)_rayDir* rayGroundOffset;

			//Logic Error : Ignore중인 땅까지 스캔하고 있음. RaycastAll을 사용?
			RaycastHit2D hit;
			hit = Physics2D.Raycast(origin, _rayDir, (groundDist+ rayGroundOffset) * 2, groundLayer);
			if (hit.collider != null) {
				if (hit.distance - rayGroundOffset * 0.5f > 0)
					isAllRayOverFoot = false;
				dist = Mathf.Min(dist,hit.distance- rayGroundOffset);
				raycastHitGround = hit;
				Color color;
				if (dist < 0)
					color = Color.blue;
				else
					color = Color.red;
				Debug.DrawLine(origin, hit.point, color);
				//Debug.DrawLine(hit.point, hit.point+ hit.normal, Color.cyan);
			} else {
				isAllRayOverFoot = false;
			}
		}
		return dist;
	}

	//발 밑바닥으로부터 offset만큼 떨어진 위치에서 지형까지의 거리 검사
	//Child 인공지능이 사용한다
	public float CheckRayDistance(Vector2 offset) {
		RaycastHit2D hit;
		float dist = 50;
		Vector2 origin = foot.transform.position
				- foot.transform.up * foot.size.y * 0.5f
				+ (Vector3)offset;
		hit = Physics2D.Raycast(origin, Vector2.down, dist, groundLayer);
		if (hit.collider != null) {
			dist = hit.distance;
			Debug.DrawLine(origin, hit.point, Color.red);
		}
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
	public bool WallCheck(int dir = 1, GameObject findTarget = null, float offsetSide = 0.1f) {
		Vector2 pos, size, up, right, rayDir;
		pos = body.transform.position;
		size = body.size;
		right = body.transform.right;
		up = body.transform.up;
		rayDir = right * dir;

		float maxWallDist = 0.15f;
		float dist = Mathf.Infinity;
		isAllRayOverFoot = true;

		//여러군데 검사
		float iAdd = 1.0f/5.0f;
		for (float i = -0.3f; i <= 0.5f; i += iAdd) {
			Vector2 origin = pos
				+ right * dir * (size.x * 0.5f - offsetSide)
				+ up * (size.y * i);

			RaycastHit2D[] hits = Physics2D.RaycastAll(origin, rayDir, maxWallDist + offsetSide, groundLayer);
			foreach (RaycastHit2D hit in hits) {
				//Ingore중인 One-Way 플랫폼은 무시
				if (IgnoreColliders.Contains(hit.collider))
					continue;
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
	public bool IsInLadder(float offset = 0, InteractionObject ladder = null) {
		if (ladder == null) ladder = interactionObject;
		if (ladder == null) return false;

		Vector3 ladderBottom, ladderTop, CurrPos;
		//offset만큼 양쪽 범위 늘리기
		ladderBottom = ladder.transform.position - ladder.transform.up * (ladder.size.y * 0.5f + offset);
		ladderTop = ladderBottom + ladder.transform.up * (ladder.size.y + offset*2);
		CurrPos = transform.position;
		if (Vector3.Dot(ladderBottom - CurrPos, ladderTop - CurrPos) <= 0) {
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
	public Vector3 GetDirectionToLadder(InteractionObject ladder = null) {
		Vector3 ret = Vector3.zero;
		if (ladder == null) ladder = interactionObject;
		if (ladder == null) return ret;

		Vector3 ladderBottom, ladderTop;
		ladderBottom = ladder.transform.position - ladder.transform.up * ladder.size.y * 0.5f;
		ladderTop = ladderBottom + ladder.transform.up * ladder.size.y;
		//Debug.DrawLine(ladderBottom, ladderTop, Color.red);

		Vector3 targetPos = GetProjectionPoint(ladderBottom, ladderTop, transform.position);
		//Debug.DrawLine(transform.position, targetPos, Color.red);

		ret = targetPos - transform.position;
		return ret;
	}
	#endregion

	//투영벡터
	public Vector3 GetProjectionPoint(Vector3 Start, Vector3 End, Vector3 Point) {
		Vector3 dir = Vector3.Normalize(End - Start);
		Vector3 ret = Start + dir * Vector3.Dot(Point - Start, dir);
		return ret;
	}
}
