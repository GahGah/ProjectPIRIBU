using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine {

	public T GetChildClass<T>() where T : StateMachine {
		return (T)this;
	}

	State currentState;
	public virtual void SetState() {
	}
	void Update() {
	}

}

public class CharacterStateMachine : StateMachine {
	public Character character;
	public void SetCharacter(Character _character) {
		character = _character;
	}

	public CharacterStateMachine(Character _character) {
		SetCharacter(_character);
	}

}
