using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : CharacterState {
	public Enemy enemy;
	public override void SetCharacter(Character _character) {
		_character.GetChildClass<Enemy>();
	}
}

public class EnemyIdle : EnemyState {
}