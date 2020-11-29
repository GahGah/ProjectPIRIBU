using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Child : Character {
	public float allowedCliffSpace = 0.1f;
	protected override void Start() {
		base.Start();
		stateMachine = new CharacterStateMachine(this, States.Child_Air);
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
		moveStat = unit.status;
	}
}

public class ChildGround : ChildState {

	public override void Enter() {
		moveStat.verticalSpeed = 0;
	}
	public override void Execute() {
		
		Vector2 groundForward = Vector2.right;

		//지형 부착
		if (unit.AttachGround()) {
			groundForward = unit.groundForward;
		} else {
			sm.SetState(States.Child_Air);
		}

		//좌우이동 인공지능
		float followStartDist = 1f;//이동 시작하는 최소값
		float safetyDistance = 1.5f;//추락지점 이격거리

		float heroXDist = hero.unit.transform.position.x - unit.transform.position.x;
		int heroDir = Mathf.Abs(heroXDist) >= followStartDist ? (heroXDist > 0 ? 1 : -1) : 0;
		float dist = unit.GetWalkableDistance(heroDir, safetyDistance*1.5f, child.allowedCliffSpace);
		int moveDir = dist >= safetyDistance ? heroDir : 0;

		//임시로 컨트롤 버튼 누르고 있을때만 follow
		//if (!input.buttonCtrl.isPressed) moveDir = 0;

		unit.HandleMoveSpeed(moveDir, moveStat.groundMoveSpeed);
		
		//이동호출
		Vector2 vel = groundForward * moveStat.sideMoveSpeed;
		
		unit.SetMovement(MovementType.SetVelocity, vel);

	}
}

public class ChildAir : ChildState {
	public override void Enter() {
	}
	public override void Execute() {

		
		//좌우이동
		unit.HandleMoveSpeed(0, moveStat.airMoveSpeed);

		//추락
		moveStat.verticalSpeed -= moveStat.fallSpeed;

		//이동호출
		Vector2 vel = new Vector2(moveStat.sideMoveSpeed, moveStat.verticalSpeed);
		unit.SetMovement(MovementType.SetVelocity, vel);


		//착지판정
		if (unit.GroundCheckFromAir())
			sm.SetState(States.Child_Ground);
	}

}
