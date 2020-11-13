using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character는 Ground나 Platform을 딛고 서있을 수 있다.
/// </summary>
public class Character : MonoBehaviour
{
	public UnitCharacter unit;
	public StateMachine stateMachine;
	public Animator animator;

	public Collider2D footCollider;
	public Collider2D SensorCollider;
	public StageObjectSensor sensor;

	public CharacterStatus status;

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

	public void ManageFootCollider()
    {
		Vector3 velocity = unit.rigid.velocity; //캐릭터의 움직임 방향벡터
		Vector3 footPos = footCollider.transform.position;

		//현재 맞닿아잇는 플랫폼들을 순회
		foreach (LinearPlatform platform in sensor.linearPlatforms) {
			// One-way 플랫폼일 경우
			if (platform.isOneWay)
			{
				// One-Way 플랫폼 충돌무시 검사
				Physics2D.IgnoreCollision(
					footCollider, platform.GetComponent<Collider2D>(),
					platform.GetIsOneWayIgnore(footPos,velocity));
			}
		}
    }
	
   
	
    //-------------------------------
}
