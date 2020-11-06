using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildState : CharacterState {
	public Child Child;
	public override void SetCharacter(Character _character) {
		_character.GetChildClass<Child>();
	}
}

public class ChildIdle : ChildState {
}

public class ChildMove : ChildState {

}