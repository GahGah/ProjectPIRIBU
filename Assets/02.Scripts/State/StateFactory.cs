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
			case States.Player_Jump:
				retState = new PlayerJump();
				break;
			case States.Player_Air:
				retState = new PlayerAir();
				break;
			default:
				Debug.LogWarning("StateFactory : " + _state.ToString() + "객체를 생성하지 못했습니다.");
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
