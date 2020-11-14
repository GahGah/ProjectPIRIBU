using System;
using System.Collections;
using System.Collections.Generic;
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
	public CharacterStatus status;


	public T GetChildClass<T>() where T : Character {
		return (T)this;
	}

	protected virtual void Start() {
		unit.character = this;
		//StateMachine 초기화는 자식 객체들이 한다.
	}

	protected virtual void FixedUpdate() {
		stateMachine.Update();
		unit.ManageFootCollider();
	}

}
