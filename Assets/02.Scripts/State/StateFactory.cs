using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFactory : SingleTon<StateFactory> {

	public State GetState(StateMachine _sm, States _state) {
		State retState = null;
		switch (_state) {
			case States.Player_Idle:
				retState = new PlayerIdle();
				break;
			default:
				break;
		}

		if (retState != null) {
			retState.SetStateMachine(_sm);
			retState.Init();
		}
		return retState;
	}
	//StateMachine마다 전달해줘야 하는 매개인자가 다르다. <= 보류
	//public virtual void SetParameters(List<object> _parameters) {}

	//State 생성 함수. StateMachine이 요청할 것이다.

}

public enum States { 
	Player_Idle, Player_Walk, Player_Air, Player_Jump,
	Child_Idle, Child_Walk, Child_Air, Child_Jump
}
