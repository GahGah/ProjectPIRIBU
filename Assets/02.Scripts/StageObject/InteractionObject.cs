using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum InteractionType {
	Ladder,PushBox
}
public class InteractionObject : LiftObject { 
	
	[HideInInspector] public Vector2 size;
	[HideInInspector] public InteractionType type;
	[HideInInspector] public Joint2D joint;
	protected override void Awake() {
		base.Awake();
		gameObject.tag = "InteractionObject";
	}
}
