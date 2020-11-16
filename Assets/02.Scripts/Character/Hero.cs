using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Hero : Character {

	protected override void Start() {
		base.Start();
		stateMachine = new CharacterStateMachine(this,States.Hero_Ground);
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
}

public class HeroGround : HeroState {
	float tick;
	public override void Enter() {
		tick = 0f;
	}
	public override void Execute() {
		tick += Time.deltaTime;

		//지형 부착
		//hero.unit.RayGround(Vector2.down);

		//좌우이동
		int moveDir = 0;
		if (input.buttonLeft.isPressed) moveDir = -1;
		if (input.buttonRight.isPressed) moveDir = 1;

		hero.HandleMoveSpeed( moveDir, charStat.groundMoveSpeed);

		Vector2 vel = hero.unit.GetVelocity();
		vel = new Vector2(charStat.sideMoveSpeed, vel.y - charStat.fallSpeed);
		hero.unit.SetMovement(MovementType.SetVelocity, vel);

		if (input.buttonJump.isPressed) {
			sm.SetState(States.Hero_Jump);
		}
	}
}

public class HeroJump : HeroState {
	float tick;
	public override void Enter() {
		hero.unit.SetLiftParent(null);
		Debug.Log("Jump!");
		tick = 0;
		hero.unit.SetMovement(MovementType.AddVelocity, Vector2.up * 30);
	}
	public override void Execute() {
		tick += Time.deltaTime;
		if (tick >= 0.1f) {
			sm.SetState(States.Hero_Ground);
		}
	}
}

public class HeroAir : HeroState {

}