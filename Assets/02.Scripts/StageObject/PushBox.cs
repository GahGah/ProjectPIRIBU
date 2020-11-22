using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBox : InteractionObject
{
	protected override void Awake() {
		base.Awake();
		type = InteractionType.PushBox;
		size = transform.localScale;
	}

}
