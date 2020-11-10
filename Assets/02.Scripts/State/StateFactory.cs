using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateFactory : SingleTon<StateFactory> {

	public State GetState(StateMachine _sm, States _state) {
		State retState = null;
		switch (_state) {
			case States.Hero_Ground:
				retState = new HeroGround();
				break;
			case States.Hero_Jump:
				retState = new HeroJump();
				break;
			case States.Hero_Air:
				retState = new HeroAir();
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
	Hero_Ground, Hero_Air, Hero_Jump,
	Child_Idle, Child_Walk, Child_Air, Child_Jump,
	Enemy_Idle, Enemy_Walk
}
