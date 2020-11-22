using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class Ladder : InteractionObject {

	protected override void Awake() {
		base.Awake();
		type = InteractionType.Ladder;
		size = transform.localScale;
	}


}
