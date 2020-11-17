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
		//if (parent) parent.Draw();
	}

	
	public RaycastHit2D RayGround(Vector2 _dir) {
		RaycastHit2D hit;
		Vector2 origin = transform.position;
		hit = Physics2D.Raycast(origin, _dir, 0, groundLayer);
		Debug.DrawLine(origin, hit.point, Color.red);
		return hit;
	}

}
