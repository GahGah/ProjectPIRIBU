using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBox : InteractionObject
{
	public Collider2D[] frictionFaces;
	public PhysicsMaterial2D frictionMaterial;
	public PhysicsMaterial2D nonFrictionMaterial;
	[HideInInspector] public float groundAngle = 55;
	[HideInInspector] public bool isPushingMode = false;
	protected override void Awake() {
		base.Awake();
		type = InteractionType.PushBox;
		size = transform.localScale;
	}

	protected override void FixedUpdate() {
		base.FixedUpdate();
		foreach (Collider2D face in frictionFaces) {
			if (isPushingMode) {
				face.sharedMaterial = nonFrictionMaterial;
			} else {
				face.sharedMaterial = frictionMaterial;
				if (Vector2.Angle(-face.transform.up,Vector2.up) <=  groundAngle) {
					face.gameObject.SetActive(true);
				} else {
					face.gameObject.SetActive(false);
				}
			}
		}
	}
}
