using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFactory : SingleTon<StateFactory> {

	public State GetState(StateMachine _sm, States _state) {
		State retState = null;
		switch (_state) {
			case States.Player_Ground:
				retState = new PlayerGround();
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
}

public enum States { 
	Player_Ground, Player_Air, Player_Jump,
	Child_Idle, Child_Walk, Child_Air, Child_Jump,
	Enemy_Idle, Enemy_Walk
}
