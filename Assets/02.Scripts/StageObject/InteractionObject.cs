using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public enum InteractionType {
	Ladder,PushBox
}
public class InteractionObject : MonoBehaviour { 
	
	public T GetChildObject<T>() where T : InteractionObject {
		return (T)this;
	}
	[HideInInspector] public Vector2 size;
	[HideInInspector] public InteractionType type;
	[HideInInspector] public Joint2D joint;
	[HideInInspector] public Rigidbody2D rigid;
	protected virtual void Awake() {
		gameObject.tag = "InteractionObject";
	}
}
