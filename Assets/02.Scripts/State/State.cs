using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State {

	StateMachine sm;
	public State SetStateMachine(StateMachine _sm) {
		sm = _sm;
		return this;
	}
	public virtual void Enter() { }
	public virtual void Execute() { }
	public virtual void Exit() { }
	public virtual void HandleInput(Input _input) { }
	public virtual void HandleEvent(Event _event) { }
}

public class CharacterState : State {
	public virtual void SetCharacter(Character _character) { }
}