using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine {

	public T GetChildClass<T>() where T : StateMachine {
		return (T)this;
	}

	State currentState;
	protected Dictionary<States, State> stateDict = new Dictionary<States, State>();
	public virtual void SetState(States _state) {
		//탈출
		if (currentState != null) {
			currentState.Exit();
		}
		//State 객체 게으른 생성
		if (!stateDict.ContainsKey(_state)) {
			State requstedState;
			requstedState = StateFactory.Instance.GetState(this, _state);
			if (requstedState != null) {
				stateDict[_state] = requstedState;
			}
		}
		//진입
		if (stateDict.ContainsKey(_state)) {
			currentState = stateDict[_state];
			currentState.Enter();
		} else {
			Debug.LogWarning("StateMachine : " + _state.ToString() + "로 전이하지 못했어요.");
		}
	}
	//실행
	public virtual void Update() {
		if (currentState != null) { 
			currentState.Execute(); 
		}
	}

}


