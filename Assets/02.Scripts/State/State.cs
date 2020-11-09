using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State {

	public T GetChildClass<T>() where T : State {
		return (T)this;
	}

	protected StateMachine sm;
	public void SetStateMachine(StateMachine _sm) {
		sm = _sm;
	}
	public virtual void Init() {
	}

	public virtual void Enter() { }
	public virtual void Execute() { }
	public virtual void Exit() { }
	public virtual void HandleInput(Input _input) { }
	public virtual void HandleEvent(Event _event) { }
}

