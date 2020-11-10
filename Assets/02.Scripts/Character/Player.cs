using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {

	private void Start() {
		stateMachine = new CharacterStateMachine(this,States.Player_Ground);
	}

	private void FixedUpdate() {
		stateMachine.Update();
	}
}