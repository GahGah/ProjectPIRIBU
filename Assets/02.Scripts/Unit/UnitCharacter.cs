using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class UnitCharacter : Unit
{
	public Rigidbody2D rigid;
	[SerializeField] protected LayerMask groundLayer;
	[SerializeField] protected LayerMask platformLayer;
	public RaycastHit2D RayGround(Vector2 _dir) {
		RaycastHit2D hit;
		Vector2 origin = transform.position;
		hit = Physics2D.Raycast(origin, _dir, 0, groundLayer);
		Debug.DrawLine(origin, hit.point, Color.red);
		return hit;
	}

	public CharacterStatus status;
}
