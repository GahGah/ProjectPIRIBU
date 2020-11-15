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
				inst.firstDegree = i * Mathf.Deg2Rad;
				inst.standardPos = standardPos;
			}
		}
	}
	protected override void Update() {
		targetPos =
			standardPos +
			Vector3.left * Mathf.Sin(firstDegree + Time.time * 0f) * 5f
			+ Vector3.up * Mathf.Cos(firstDegree + Time.time * 0f) * 5f;
		SetMovement(MovementType.SetTargetPos, targetPos);
	}
}