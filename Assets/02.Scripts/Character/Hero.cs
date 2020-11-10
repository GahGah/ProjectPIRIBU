using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	public override void Init() {
		base.Init();
		hero = character.GetChildClass<Hero>();
	}
}

public class HeroGround : HeroState {
	float tick;
	public override void Enter() {
		tick = 0f;
	}
	public override void Execute() {
		tick += Time.deltaTime;
		hero.unit.rigid.velocity += Vector2.down * 10f * Time.deltaTime;
		//Hero.unit.transform.position += Vector3.left*Time.deltaTime;
		if (tick >= 1.5f) {
			sm.SetState(States.Hero_Jump);
		}
	}
}

public class HeroJump : HeroState {
	float tick;
	public override void Enter() {
		Debug.Log("Jump!");
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