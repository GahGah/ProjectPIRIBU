using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
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
	public virtual void HandleInput(InputType type, object value) {
	}

	public enum InputType {
		Input, Select
	}
}

