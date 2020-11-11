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
		//ManageFootCollider();
		ManageCollider_ver2();
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

	// 아래부터 변경점 :
	// 현재 맞닿아있는 플랫폼의 정보값을 가져오는게 어떨지.
	// 이 경우에는 발에 맞닿아있는 플랫폼이 움직이는 경우 함께 움직이게 하는 것도 dir*speed를 가져오면 되니 구현 가능할 것 같음. 
	// (dir*speed를 사용하지 않는다고 하도 nowPos - curPos를 해서 가져와 플레이어에게 넣는 방법도 있을 듯.)
	// One-Way 플랫폼을 움직이게 할 때(리프트), 내부 코드에서 target 좌표. 그리고 그 방향에 도달하기 위한 speed와 Direction을 정해준다고 가정.
	// 즉, target좌표까지 update(){ transform.position += speed * Direction; } 일 경우. (rigid를 사용할 경우도 velocity로 가져오면 될 듯)

	private List<GameObject> platforms = new List<GameObject>();
	public CapsuleCollider2D bodyCol;
	public CapsuleCollider2D triggerCol;

	public void ManageCollider_ver2()
    {
		Vector2 charDirV = unit.rigid.velocity; //캐릭터의 움직임 방향벡터.
		Vector2 platformDirV; //플랫폼의 움직임 방향벡터.
        Vector2 platformUpV; //플랫폼이 가지는 윗면의 방향 벡터.(플랫폼의 각도에 영향을 받음) 

		footCollider.enabled = false;

		//현재 맞닿아잇는 플랫폼들을 순회.
		foreach (GameObject platform in platforms)
		{
			platformUpV = platform.transform.up;

			// One-way 플랫폼일 경우.
			if (platform.CompareTag("oneWayPlatform"))
			{
				// 캐릭터의 움직임 방향벡터와 플랫폼의 윗면 방향벡터의 내적 체크.
				if (Vector3.Dot(charDirV, platformUpV) > 0) 
				{
					Physics2D.IgnoreCollision(bodyCol, platform.GetComponent<Collider2D>(), true); //두 콜라이더의 충돌체크를 무시함.
				}
				else
				{
					Physics2D.IgnoreCollision(bodyCol, platform.GetComponent<Collider2D>(), false); //두 콜라이더의 충돌체크를 허용함.
				}
			}
		}
    }
	
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
			platforms.Add(collision.gameObject);
			Debug.Log(platforms);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
		if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			platforms.Remove(collision.gameObject);
		}
	}
	
    //-------------------------------
}
