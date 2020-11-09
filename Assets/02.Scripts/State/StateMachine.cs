using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine {

	public T GetChildClass<T>() where T : StateMachine {
		return (T)this;
	}


	State currentState;
	protected Dictionary<States, State> stateDict;
	public virtual void SetState(States _state) {
		//탈출
		if (currentState != null) {
			currentState.Exit();
		}
		//진입
		if (!stateDict.ContainsKey(_state)) {
			//State 객체 게으른 생성
			stateDict[_state] = StateFactory.Instance.GetState(this, _state);
		}
		if (stateDict.ContainsKey(_state)) {
			currentState = stateDict[_state];
			currentState.Enter();
		}
	}
	public virtual void Update() {
		currentState.Execute();
	}

}


