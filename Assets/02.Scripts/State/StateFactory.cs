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
			case States.Hero_Ladder:
				retState = new HeroLadder();
				break;
			case States.Hero_Box:
				retState = new HeroBox();
				break;
			case States.Hero_Die:
				retState = new HeroDie();
				break;
			case States.Child_Ground:
				retState = new ChildGround();
				break;
			case States.Child_Air:
				retState = new ChildAir();
				break;
			case States.Enemy_Ground:
				retState = new EnemyGround();
				break;
			case States.Enemy_Air:
				retState = new EnemyAir();
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
	Hero_Ground, Hero_Air, Hero_Jump, Hero_Ladder, Hero_Box, Hero_Die,
	Child_Ground,Child_Air,
	Enemy_Ground, Enemy_Air
}
