using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//캐릭터 물리담당
[RequireComponent(typeof(Rigidbody2D))]
public class UnitCharacter : Unit
{
	[SerializeField] protected LayerMask groundLayer;
	public Rigidbody2D rigid;
	[HideInInspector] public Character character;

	public CharacterFoot foot;
	public StageObjectSensor sensor;
	protected LiftObject liftParent;//사다리, Lift 지형 등의 객체

	public void ManageFootCollider() {
		Vector3 footPos = foot.transform.position;

		//플랫폼 순회
		foreach (LinearPlatform platform in sensor.linearPlatforms) {
			// One-way 플랫폼일 경우
			if (platform.isOneWay) {
				bool isIgnore = false;
				//부모는 밟고있어야 한다.
				if (platform == liftParent) {
					isIgnore = platform.GetIsOneWayIgnore(footPos);
					isIgnore = false;
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

		//Lifting Platform
		LiftObject lift = null;
		if (foot.adjacentlinearPlatforms.Count > 0) {
			lift = foot.adjacentlinearPlatforms[0];
		}
		SetLiftParent(lift);
		if (lift)
			lift.Draw();
	}

	//LiftParent 설정
	protected void SetLiftParent(LiftObject lift) {
		//다른 parent로 전이
		if (liftParent != lift) {
			if (liftParent)
				liftParent.RemoveChild(character.transform);
			liftParent = lift;
			if (liftParent)
				liftParent.AddChild(character.transform);
		}
	}
	public RaycastHit2D RayGround(Vector2 _dir) {
		RaycastHit2D hit;
		Vector2 origin = transform.position;
		hit = Physics2D.Raycast(origin, _dir, 0, groundLayer);
		Debug.DrawLine(origin, hit.point, Color.red);
		return hit;
	}

}
