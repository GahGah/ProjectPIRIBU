using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : CharacterState {
	public Enemy enemy;
	public override void Init() {
		base.Init();
		enemy = character.GetChildClass<Enemy>();
	}
}

public class EnemyIdle : EnemyState {
}