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
		float time = Time.time * 0.5f;
		float radious = 8f;
		Vector3 pos =
			standardPos +
			Vector3.left * Mathf.Sin(firstDegree + time) * radious
			+ Vector3.up * Mathf.Cos(firstDegree + time) * radious;
		SetMovement(MovementType.SetTargetPos, pos);
	}
}