using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

	protected override void Start() {
		base.Start();
		stateMachine = new CharacterStateMachine(this,States.Player_Ground);
	}

	protected override void FixedUpdate() {
		base.FixedUpdate();
	}
}