using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftObject : MonoBehaviour
{
	public List<Transform> childs;//옮길 대상

	[HideInInspector] public Collider2D coll;
	[HideInInspector] public Rigidbody2D rigid;

	[HideInInspector]
	public Vector3
		prePos, currPos, standardPos, deltaPos,
		preScale, currScale, standardScale,
		preAngle, currAngle, standardAngle,
		preNormal, currNormal, standardNormal;

	float fixedUpdatePerSec;


	protected virtual void Awake() {

		childs = new List<Transform>();
		coll = GetComponent<Collider2D>();
		rigid = GetComponent<Rigidbody2D>();

		UpdateCurrTransforms();
		UpdatePreTransforms();
		standardPos = currPos;
		standardScale = currScale;
		standardAngle = currAngle;
		standardNormal = currNormal;

		fixedUpdatePerSec = 1 / Time.fixedDeltaTime;
	}


	private void Update() {
		UpdatePreTransforms();
		UpdateTransform();
		UpdateCurrTransforms();

		rigid.velocity = deltaPos;
		deltaPos = currPos- prePos;

		foreach (Transform child in childs) {
			child.position = GetLiftPosition(child.position);
		}
	}


	//사용자 지정 이동함수
	protected virtual void UpdateTransform() {
	}

	//1프레임 전 트랜스폼 업데이트
	private void UpdatePreTransforms() {
		prePos = currPos;
		preScale = currScale;
		preAngle = currAngle;
		preNormal = currNormal;
	}

	//현재프레임 트랜스폼 업데이트
	private void UpdateCurrTransforms() {
		currPos = transform.position;
		currAngle = transform.eulerAngles;
		currScale = transform.localScale;
		currNormal = transform.up;
	}


	public void AddChild(Transform child) {
		childs.Add(child);
	}

	public void RemoveChild(Transform child) {
		childs.Remove(child);
	}

	public Vector3 GetLiftPosition(Vector3 charPos) {
		return deltaPos + charPos;
	}

	public void Draw() {
		Utility.DrawDir(currPos, deltaPos + Vector3.zero, Color.green);
	}
}
