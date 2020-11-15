using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovePlatform : LinearPlatform
{

	public float firstDegree;
	public bool isOriginal = true;
	protected override void Awake() {
		base.Awake();
	}
	private void Start() {
		firstDegree = firstDegree * Mathf.Deg2Rad;
		if (isOriginal) {
			for (int i = 30; i < 360; i += 30) {
				CircleMovePlatform inst = 
				Instantiate(this.gameObject).GetComponent<CircleMovePlatform>();
				inst.isOriginal = false;
				inst.firstDegree = i;
				inst.standardPos = standardPos;
			}
		}
	}
	protected override void Update() {
		Vector3 pos =
			standardPos +
			Vector3.left * Mathf.Sin(firstDegree + Time.time * 2f) * 8f
			+ Vector3.up * Mathf.Cos(firstDegree + Time.time * 2f) * 8f;
		SetMovement(MovementType.SetTargetPos, pos);

		Utility.DrawDir(pos, Vector3.up, Color.red);
	}
}