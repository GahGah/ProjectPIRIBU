using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType {
	None, SetTargetPos, SetVelocity, AddVelocity
}

/// <summary>
/// 게임내의 모든 움직이는 클래스 (캐릭터나 플랫폼)
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class LiftObject : MonoBehaviour
{
	[HideInInspector] public List<LiftObject> childs;//옮길 대상
	[HideInInspector] public Collider2D coll;
	[HideInInspector] public Rigidbody2D rigid;

	[HideInInspector]
	public Vector3
		prePos, currPos, targetPos, standardPos, deltaPos,
		preScale, currScale, standardScale,
		preAngle, currAngle, standardAngle,
		preNormal, currNormal, standardNormal;

	[HideInInspector]
	public float fixedUpdatePerSec;


	protected virtual void Awake() {

		childs = new List<LiftObject>();
		if (!coll) coll = GetComponent<Collider2D>();
		if (!rigid) rigid = GetComponent<Rigidbody2D>();

		UpdateCurrTransforms();
		UpdatePreTransforms();
		standardPos = currPos;
		standardScale = currScale;
		standardAngle = currAngle;
		standardNormal = currNormal;

		fixedUpdatePerSec = 1 / Time.fixedDeltaTime;
	}


	private void FixedUpdate() {
		UpdatePreTransforms();
		UpdateTransform();
		UpdateCurrTransforms();

		deltaPos = currPos- prePos;

		//자신에게 붙은 객체 위치 변경
		foreach (LiftObject child in childs) {
			child.transform.position = GetLiftPosition(child.transform.position);
		}
	}

	public Vector3 GetLiftPosition(Vector3 charPos) {
		return (Vector3)GetVelocity() + charPos;
	}


	//사용자 지정 이동함수
	Vector3 inputVector = Vector3.zero;
	MovementType inputType = MovementType.None;
	protected virtual void UpdateTransform() {

		switch (inputType) {
			case MovementType.None:
				break;
			//Transform.Position = TargetPosition과 같은 역할.
			case MovementType.SetTargetPos:
				rigid.velocity = inputVector - transform.position;
				transform.position = inputVector;
				break;
			//Rigidbody.Velocity += (초당 이동위치)와 같은 역할.
			case MovementType.AddVelocity:
				rigid.AddForce(inputVector*fixedUpdatePerSec);
				break;
			//Rigidbody.Velocity = (초당 이동위치)와 같은 역할.
			case MovementType.SetVelocity:
				rigid.AddForce( ((Vector2)inputVector-rigid.velocity)
					*fixedUpdatePerSec );
				break;
		}
		inputType = MovementType.None;
	}

	//매프레임마다 LiftObject를 움직이는 방식은 사용자 지정
	public void SetMovement(MovementType setType,Vector2 _input) {
		inputType = setType;
		inputVector = _input;
	}
	public Vector2 GetVelocity() {
		return rigid.velocity;
	}



	//1프레임 전 트랜스폼 업데이트
	protected virtual void UpdatePreTransforms() {
		prePos = currPos;
		preScale = currScale;
		preAngle = currAngle;
		preNormal = currNormal;
	}

	//현재프레임 트랜스폼 업데이트
	protected virtual void UpdateCurrTransforms() {
		currPos = transform.position;
		currAngle = transform.eulerAngles;
		currScale = transform.localScale;
		currNormal = transform.up;
	}

	public void AddChild(LiftObject child) {
		childs.Add(child);
	}

	public void RemoveChild(LiftObject child) {
		childs.Remove(child);
	}

	public void Draw() {
		Debug.DrawLine(currPos, currPos - (Vector3)GetVelocity(), Color.green, 1f);
		Debug.DrawLine(currPos, currPos + (Vector3)GetVelocity(), Color.red);
	}
}
