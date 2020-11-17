using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Child : Character {
	protected override void Start() {
		base.Start();
		stateMachine = new CharacterStateMachine(this, States.Child_Ground);
	}

	protected override void FixedUpdate() {
		base.FixedUpdate();
	}

}
public class ChildState : CharacterState {
	public Child child;
	public Hero hero;
	protected InputManager input;
	public override void Init() {
		base.Init();
		child = character.GetChildClass<Child>();
		hero = GameManager.Instance.hero;
		input = InputManager.Instance;
	}
}

public class ChildGround : ChildState {
	float tick;
	float rayDist;//지형 레이 거리
	Vector2 groundNormal;//지형 노말
	Vector2 groundForward;//GroundNormal의 직교벡터
	public override void Enter() {
		tick = 0f;
		charStat.verticalSpeed = 0;
		groundNormal = Vector2.up;
	}
	public override void Execute() {

		tick += Time.deltaTime;

		//지형 부착
		rayDist = child.unit.RayGround(Vector2.down);
		if (rayDist > child.unit.groundDist) {
			//땅과 거리차가 날시 공중상태
			//sm.SetState(States.child_Air);
		} else {
			groundNormal = child.unit.raycastHitGround.normal;
			groundForward = new Vector2(groundNormal.y, -groundNormal.x);

			child.unit.SetMovement(MovementType.AddPos, Vector2.down * rayDist);
			rayDist = child.unit.RayGround(-groundNormal);
			child.unit.SetMovement(MovementType.AddPos, Vector2.down * rayDist);
		}

		//좌우이동 인공지능
		int moveDir = 0;
		float toHero = hero.unit.transform.position.x - child.unit.transform.position.x;



		float groundDist = 0;
		if (toHero > 3) {
			for (float i = 0; i < toHero; i++) {
				groundDist = child.unit.CheckRayDistance(Vector2.right * i + Vector2.down * groundDist);

			}
			moveDir = 1; }
		else if (toHero < -3) {
			for (float i = 0; i > toHero; i--) {
				groundDist = child.unit.CheckRayDistance(Vector2.right * i + Vector2.down * groundDist);
			}
			moveDir = -1; 
		}
		child.HandleMoveSpeed(moveDir, charStat.groundMoveSpeed);

		//이동호출
		Vector2 vel = groundForward * charStat.sideMoveSpeed
			+ Vector2.down*5f;
		//Vector2 vel = new Vector2(charStat.sideMoveSpeed, 0);
		child.unit.SetMovement(MovementType.SetVelocity, vel);

	}
}
