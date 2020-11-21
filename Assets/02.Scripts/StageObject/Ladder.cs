using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class Ladder : InteractionObject {

	protected override void Awake() {
		base.Awake();
		rigid = GetComponent<Rigidbody2D>();
		type = InteractionType.Ladder;
		size = transform.localScale;
	}


}
