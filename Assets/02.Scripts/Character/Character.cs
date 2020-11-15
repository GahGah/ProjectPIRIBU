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
	
	//속도 처리 함수
	public void HandleMoveSpeed(int moveDir, MoveSpeed moveSpeed) {
		float speed = status.sideMoveSpeed;
		
		int speedDir = (speed > 0 ? 1 : -1);

		if (moveDir != 0) {
			//가속
			speed += moveDir * moveSpeed.accel;
		} else {
			//브레이크
			if (speed < moveSpeed.brake)
				speed = 0;
			else
				speed -= speedDir * moveSpeed.brake;
		}
		//최대속력 제한
		if (speed * speedDir > moveSpeed.max) {
			speed = speedDir * moveSpeed.max;
		}

		status.sideMoveSpeed = speed;
	}


	protected virtual void Start() {
		unit.character = this;
		//StateMachine 초기화는 자식 객체들이 한다.
	}

	protected virtual void FixedUpdate() {
		unit.ManageFootCollider();
		stateMachine.Update();
	}

}
