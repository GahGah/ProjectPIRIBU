using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerState : CharacterState {
	public Player player;
	public override void Init() {
		base.Init();
		player = character.GetChildClass<Player>();
	}
}

public class PlayerIdle : PlayerState {
}

public class PlayerMove : PlayerState {
}

public class PlayerJump : PlayerState {
}

public class PlayerAir : PlayerState {

}