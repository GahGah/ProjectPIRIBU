using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;

/// <summary>
/// Character는 Ground나 Platform을 딛고 서있을 수 있다.
/// </summary>
public class Character : MonoBehaviour
{
	public UnitCharacter unit;
	public StateMachine stateMachine;
	public Animator animator;

	public T GetChildClass<T>() where T : Character {
		return (T)this;
	}


	protected virtual void Start() {
		unit.character = this;
		//StateMachine 초기화는 자식 객체들이 한다.
	}

	protected virtual void FixedUpdate() {
		unit.ManageColliders();
		stateMachine.Update();
	}

	void OnDrawGizmos() {
		if (Application.isPlaying) {
			GUIStyle style = new GUIStyle();
			style.alignment = TextAnchor.MiddleCenter;
			style.fontSize = 10;
			Handles.Label(transform.position+Vector3.up*1.5f, stateMachine.GetStateName());

		}
	}
}
