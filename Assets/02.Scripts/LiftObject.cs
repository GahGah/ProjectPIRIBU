using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType {
	None, MovePos, SetVelocity, AddVelocity
}

/// <summary>
/// 게임내의 모든 움직이는 클래스 (캐릭터나 플랫폼)
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class LiftObject : MonoBehaviour {

	public LiftObject parent;
	[HideInInspector] public List<LiftObject> childs;//옮길 대상
	[HideInInspector] public Collider2D coll;
	[HideInInspector] public Rigidbody2D rigid;

	[HideInInspector]
	public Vector3
		prePos, currPos, targetPos, standardPos, deltaPos, nextPos,
		preScale, currScale, standardScale,
		preAngle, currAngle, standardAngle,
		preNormal, currNormal, standardNormal;
	/* Previous : 이전 프레임의, Current : 현재 프레임의, Target : 목표의
	 * Standard : 기준이 되는
	 * Delta : 이전 프레임과 얼마나 차이나는지
	 * Next : 다음에 어디에 있는지 (추론적, 확실하지 않음.)
	 */

	[HideInInspector]
	public float fixedUpdatePerSec;

	private float updatedTime = 0;//업데이트된 시각

	protected virtual void Awake() {
		parent = null;
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

		UpdateNow();
	}

	private void UpdateNow() {
		if (updatedTime == Time.time) return;
		updatedTime = Time.time;
		//bool isHero = gameObject.name == "Hero";
		//if (isHero) Debug.Log(parent);

		if (parent) 
			SetMovement(MovementType.MovePos, parent.GetLiftPosition(this));
		
		//현재 트랜스폼 정보 업데이트
		UpdateCurrTransforms();
		deltaPos = currPos - prePos;

		//UpdateRigidbody에선 Transform이 변경되지 않는다.
		//따라서 UpdateCurrTransforms가 먼저 와도 값의 차이가 없음.
		UpdateRigidbody();
		movementInputs.Clear();

	}

	public Vector3 GetLiftPosition(LiftObject child) {
		//자신의 물리 업데이트 먼저!
		UpdateNow();
		return deltaPos + child.transform.position;

		//MovePos함수 호출로 이동하면 다음에 어디로 갈지 정확히는 모른다.
		//따라서 다음에 어디에 존재할지 예측하는 NextPos를 사용?
	}

	//요청받은 이동함수 리스트에 맞추어 Rigidbody 관련 함수 호출.
	//이 함수단계에서 당장 Transform의 변화는 일어나지 않고, 다음 프레임에 변경된다.
	struct MovementInput {
		public MovementType type;
		public Vector2 vector;
		public MovementInput(MovementType _type, Vector2 _vec) {
			type = _type;
			vector = _vec;
		}
	}
	private List<MovementInput> movementInputs = new List<MovementInput>();

	protected virtual void UpdateRigidbody() {
		if (movementInputs.Count == 0) return;

		Vector2 movePos = currPos;
		Vector2 totalVel = Vector2.zero;
		Vector2 targetVel = rigid.velocity;

		foreach (MovementInput input in movementInputs) {
			switch (input.type) {
				case MovementType.None:
					break;
				//Transform.Position = TargetPosition과 같은 역할.
				case MovementType.MovePos:
					movePos = input.vector;
					break;
				//Rigidbody.Velocity += (초당 이동위치)와 같은 역할.
				case MovementType.AddVelocity:
					totalVel += input.vector;
					break;
				//Rigidbody.Velocity = (초당 이동위치)와 같은 역할.
				case MovementType.SetVelocity:
					targetVel = input.vector;
					break;
			}
		}

		float mult = rigid.mass * fixedUpdatePerSec;
		Vector2 addVel = (totalVel + (targetVel - rigid.velocity));
		//MovePos
		if (movePos != (Vector2)transform.position) {
			if (addVel == Vector2.zero) {
				rigid.MovePosition(movePos);//MovePos만 하는 경우
			} else {
				rigid.MovePosition(
				movePos
				//+ addVel * (-Time.fixedDeltaTime)//addVel 상쇄
				+ targetVel * Time.fixedDeltaTime//targetVel만큼 이동
				+ Vector2.down * 0.2f//땅에 안붙어서 땜빵처리. 이렇게 구현하면 안된다!!!!
				);
			}
		} else {
			rigid.AddForce(addVel * mult);//AddForce만 하는 경우
		}

		nextPos = movePos;
	}

	//매프레임마다 LiftObject를 움직이는 방식은 사용자 지정
	public void SetMovement(MovementType inputType, Vector2 inputVec) {
		movementInputs.Add(new MovementInput(inputType, inputVec));
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

	//LiftParent 설정
	public void SetLiftParent(LiftObject lift) {
		parent = lift;
	}

	public void ResetLiftParent() {
		parent = null;
	}

	public void Draw() {
		Debug.DrawLine(currPos, currPos - (Vector3)GetVelocity(), Color.green, 1f);
		Debug.DrawLine(currPos, currPos + (Vector3)GetVelocity(), Color.red);
	}
}
