using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBox : InteractionObject
{
	protected override void Awake() {
		base.Awake();
		rigid = GetComponent<Rigidbody2D>();
		type = InteractionType.PushBox;
		size = transform.localScale;
	}

}
