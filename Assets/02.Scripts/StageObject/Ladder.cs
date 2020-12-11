using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.XR.WSA.Input;
using UnityEngine.U2D;

[ExecuteInEditMode]
public class Ladder : InteractionObject {

	public SpriteShapeController ladderShape;
	protected override void Awake() {
		base.Awake();
		type = InteractionType.Ladder;
		size = transform.localScale;
		Spline spline = ladderShape.spline;

		Vector3 scale = transform.localScale;
		scale.x = 1;
		transform.localScale = scale;
		ladderShape.transform.localScale = new Vector3(1, 1 / scale.y, 1);
		spline.SetPosition(0, Vector3.down * transform.localScale.y * 0.5f);
		spline.SetPosition(1, Vector3.up * transform.localScale.y * 0.5f);

	}

	private void OnDrawGizmos() {
		Awake();
	}
}
