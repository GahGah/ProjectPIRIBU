using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class Ladder : InteractionObject {
	float sqrDist = 10 * 10;

	private void Awake() {
		type = InteractionType.Ladder;
	}
	void FixedUpdate() {
	}

}
