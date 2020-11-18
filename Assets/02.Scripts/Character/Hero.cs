using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hero : Character {

	protected override void Start() {
		base.Start();
		stateMachine = new CharacterStateMachine(this, States.Hero_Ground);
	}

	protected override void FixedUpdate() {
		base.FixedUpdate();
	}

}

public class HeroState : CharacterState {
	public Hero hero;
	protected InputManager input;
	public override void Init() {
		base.Init();
		hero = character.GetChildClass<Hero>();
		input = InputManager.Instance;
	}
	public void Draw(Color color) {
		Debug.DrawLine(hero.transform.position, hero.transform.position + hero.transform.up, color);
	}
}

//지형 상태
public class HeroGround : HeroState {
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
		Draw(Color.green);

		tick += Time.deltaTime;

		//지형 부착
		if (hero.unit.RayAttachGround()) {
			groundForward = hero.unit.groundForward;
		} else {
			//땅과 거리차가 날시 공중상태
			sm.SetState(States.Hero_Air); 
		}

		//좌우이동
		int moveDir = 0;
		if (input.buttonLeft.isPressed) moveDir = -1;
		if (input.buttonRight.isPressed) moveDir = 1;
		hero.HandleMoveSpeed(moveDir, charStat.groundMoveSpeed);

		
		//이동호출
		Vector2 vel = groundForward * charStat.sideMoveSpeed;
		//Vector2 vel = new Vector2(charStat.sideMoveSpeed, 0);
		hero.unit.SetMovement(MovementType.SetVelocity, vel);

		if (input.buttonJump.isPressed) {
			sm.SetState(States.Hero_Jump);
		}
	}
}

public class HeroJump : HeroState {
	public override void Enter() {
		hero.unit.ResetLiftParent();
		hero.unit.foot.adjacentlinearPlatforms.Clear();
		charStat.verticalSpeed = charStat.jumpSpeed;
		sm.SetState(States.Hero_Air);
	}
	public override void Execute() {
	}
}

//공중 상태
public class HeroAir : HeroState {
	public override void Enter() {
	}
	public override void Execute() {
		Draw(Color.yellow);
		//좌우이동
		int moveDir = 0;
		if (input.buttonLeft.isPressed) moveDir = -1;
		if (input.buttonRight.isPressed) moveDir = 1;
		hero.HandleMoveSpeed(moveDir, charStat.airMoveSpeed);

		//추락
		charStat.verticalSpeed -= charStat.fallSpeed;

		//이동호출
		Vector2 vel = new Vector2(charStat.sideMoveSpeed, charStat.verticalSpeed);
		hero.unit.SetMovement(MovementType.SetVelocity, vel);

		//지형 부착
		float dist = hero.unit.RayGround(Vector2.down);
		float groundYSpeed = 0;
		if (hero.unit.raycastHitGround.rigidbody) {
			groundYSpeed = hero.unit.raycastHitGround.rigidbody.velocity.y;
		}

		if (dist < hero.unit.groundDist
			&& charStat.verticalSpeed-groundYSpeed < 0) {
			//추락할때만 땅에 붙게 (지형 속도도 고려)
			sm.SetState(States.Hero_Ground);
		}
	}
}