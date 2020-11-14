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

		float speed = 0;
		if (input.buttonLeft.isPressed) {
			speed -= 10*50;
		}
		if (input.buttonRight.isPressed) {
			speed += 10*50;
		}

		Vector2 vel = hero.unit.rigid.velocity;
		hero.unit.rigid.velocity =
			new Vector2(0,vel.y) + (
			Vector2.right * speed + Vector2.down * 10f )
			*Time.deltaTime;

		if (input.buttonJump.isPressed) {
			sm.SetState(States.Hero_Jump);
		}

	}
}

public class HeroJump : HeroState {
	float tick;
	public override void Enter() {
		Debug.Log("Jump!");
		tick = 0;
		hero.unit.rigid.velocity += Vector2.up * 10;
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