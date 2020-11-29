using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {
	protected override void Start() {
		base.Start();
		stateMachine = new CharacterStateMachine(this, States.Enemy_Air);
	}

	protected override void FixedUpdate() {
		base.FixedUpdate();
	}

}

public class EnemyState : CharacterState {
	public Enemy enemy;
	public Hero hero;
	public override void Init() {
		base.Init();
		enemy = character.GetChildClass<Enemy>();
		hero = GameManager.Instance.hero;
		moveStat = unit.status;
	}
}

public class EnemyGround : EnemyState {

	public override void Enter() {
		moveStat.verticalSpeed = 0;
	}
	public override void Execute() {

		Vector2 groundForward = Vector2.right;

		//지형 부착
		if (unit.AttachGround()) {
			groundForward = unit.groundForward;
		} else {
			sm.SetState(States.Enemy_Air);
		}

		//좌우이동 인공지능
		int heroDir = 0;
		float followStartDist = 0f;//이동 시작하는 최소값
		if (unit.IsInSensor(hero.unit.transform.position)) {
			float heroXDist = hero.unit.transform.position.x - unit.transform.position.x;
			heroDir = Mathf.Abs(heroXDist) >= followStartDist ? (heroXDist > 0 ? 1 : -1) : 0;
		}
		

		//임시로 컨트롤 버튼 누르고 있을때만 follow
		//if (!input.buttonCtrl.isPressed) moveDir = 0;

		unit.HandleMoveSpeed(heroDir, moveStat.groundMoveSpeed);

		//이동호출
		Vector2 vel = groundForward * moveStat.sideMoveSpeed;

		unit.SetMovement(MovementType.SetVelocity, vel);

	}
}

public class EnemyAir : EnemyState {
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
			sm.SetState(States.Enemy_Ground);
	}

}
