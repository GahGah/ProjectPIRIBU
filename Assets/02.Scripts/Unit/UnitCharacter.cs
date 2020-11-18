using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCharacter : Unit
{
	[SerializeField] protected LayerMask groundLayer;
	[HideInInspector] public Character character;

	public CharacterFoot foot;
	public StageObjectSensor sensor;

	public void ManageFootCollider() {
		Vector3 footPos = foot.transform.position;

		//플랫폼 순회
		foreach (LinearPlatform platform in sensor.linearPlatforms) {
			// One-way 플랫폼일 경우
			if (platform.isOneWay) {
				bool isIgnore = false;
				//부모는 밟고있어야 한다.
				if (platform == parent) {
					Physics2D.IgnoreCollision(foot.collider, platform.coll, isIgnore);
					continue;
				} else {
					isIgnore = platform.GetIsOneWayIgnore(footPos);
					// One-Way 플랫폼 충돌무시 검사
					Physics2D.IgnoreCollision(
						foot.collider, platform.coll, isIgnore);

				}
			}
		}

		//Lifting Platform을 Parent로
		LiftObject lift = null;
		if (foot.adjacentlinearPlatforms.Count > 0) {
			lift = foot.adjacentlinearPlatforms[0];
		}
		SetLiftParent(lift);
		//if (parent) parent.Draw();
	}

	#region Ground Detecting 관련

	public RaycastHit2D raycastHitGround;
	public Vector2 groundForward;
	public float groundDist = 0.5f;//최소 지형 이격거리
	//캐릭터를 땅에 붙임. groundForward가 업데이트됨. 붙이지 못했을시 false 반환
	public bool RayAttachGround() {
		RayGround(Vector2.down);
		Vector2 groundNormal = raycastHitGround.normal;
		groundForward = new Vector2(groundNormal.y, -groundNormal.x);

		//발과 지형간의 최소거리인 rayDist값으로 지형부착 이동을 한다.
		float rayDist = RayGround(-groundNormal);
		
		//지형부착 성공
		if (groundDist > rayDist) {
			if (rayDist > 0) //일반적인 부착값
				rayDist += 0.05f;
			else //Dist가 음수면 foot가 올라가게 되어있다.
				rayDist *= 0.1f;
			SetMovement(MovementType.AddPos, -groundNormal * rayDist);
			return true;
		}

		//지형부착 실패
		return false;
	}


	float rayGroundOffset = 0.15f;//발보다 약간 위에서 쬐어주는 값
	/// <summary>
	/// 지형에 RayCast하고 거리가 가장 짧은 ray를 raycastHitGround에 저장 및 가장 짧은 거리를 반환
	/// </summary>
	public float RayGround(Vector2 _rayDir) {
		float dist = Mathf.Infinity;

		//여러군데 검사
		int rays = 1;
		for (int i = -rays; i <= rays; i ++) {
			Vector2 origin = foot.transform.position
				- foot.transform.up * foot.transform.localScale.y * 0.5f
				+ foot.transform.right * foot.transform.localScale.x * ((float)i / rays * 0.5f)
				-(Vector3)_rayDir* rayGroundOffset;
			
			RaycastHit2D hit;
			hit = Physics2D.Raycast(origin, _rayDir, (groundDist+ rayGroundOffset) * 2, groundLayer);
			if (hit.collider != null) {
				dist = Mathf.Min(dist,hit.distance- rayGroundOffset);
				raycastHitGround = hit;
				Color color;
				if (dist < 0)
					color = Color.blue;
				else
					color = Color.red;
				Debug.DrawLine(origin, hit.point, color);
				//Debug.DrawLine(hit.point, hit.point+ hit.normal, Color.cyan);
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
				- foot.transform.up * foot.transform.localScale.y * 0.5f
				+ (Vector3)offset;
		hit = Physics2D.Raycast(origin, Vector2.down, dist, groundLayer);
		if (hit.collider != null) {
			dist = hit.distance;
			Debug.DrawLine(origin, hit.point, Color.red);
		}
		return dist;
	}
	#endregion

}
