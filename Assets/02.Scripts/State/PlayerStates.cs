using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerState : CharacterState {
	public Player player;
	public override void Init() {
		base.Init();
		player = character.GetChildClass<Player>();
	}
}

public class PlayerGround : PlayerState {
	float tick;
	public override void Enter() {
		tick = 0f;
	}
	public override void Execute() {
		tick += Time.deltaTime;
		//player.unit.transform.position += Vector3.left*Time.deltaTime;
		if (tick >= 1.5f) {
			//sm.SetState(States.Player_Jump);
		}
	}
}

public class PlayerJump : PlayerState {
	float tick;
	public override void Enter() {
		Debug.Log("Jump!");
		player.unit.rigid.velocity += Vector2.up*5;
	}
	public override void Execute() {
		tick += Time.deltaTime;
		if (tick >= 0.1f) {
			sm.SetState(States.Player_Ground);
		}
	}
}

public class PlayerAir : PlayerState {

}