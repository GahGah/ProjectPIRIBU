using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildState : CharacterState {
	public Child child;
	public override void Init() {
		base.Init();
		child = character.GetChildClass<Child>();
	}
}

public class ChildIdle : ChildState {
}

public class ChildMove : ChildState {

}