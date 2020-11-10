using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	public UnitCharacter unit;
	public BoxCollider2D footCollider;
	public StateMachine stateMachine;
	public Animator animator;

	public T GetChildClass<T>() where T : Character {
		return (T)this;
	}

	protected virtual void Start() {
		//StateMachine 초기화는 자식 객체들이 한다.
	}

	protected virtual void FixedUpdate() {
		ManageFootCollider();
		stateMachine.Update();
	}
	
	//하강중일때만 One-Way 플랫폼에 닿게 한다.
	public void ManageFootCollider() {
		Vector3 charSpeed = unit.rigid.velocity;
		if (charSpeed.y > 0) {
			footCollider.enabled = false;
		} else {
			footCollider.enabled = true;
		}
	}



}
