using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RigidObject : MonoBehaviour
{
	Rigidbody2D rigid;
    void Start()
    {
		rigid = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {


		#region 합력 테스트
		// 1/fixedDeltaTime는 초당 물리연산 횟수이다.
		float mult = rigid.mass / Time.fixedDeltaTime ;
		float moveMount = 1f;

		//AddForce는 위를 향한다.
		Vector3 addVel = Vector2.up * moveMount;
		//rigid.AddForce(addVel * mult);

		//Addforce의 이동을 상쇄시킨 MovePosition의 식
		if(InputManager.Instance.buttonUp.isPressed)
		rigid.MovePosition(transform.position- addVel*Time.fixedDeltaTime);

		rigid.velocity = addVel*(1*rigid.drag);
		//두 연산이 합쳐져 오브젝트는 정지한다

		/*
		 * Velocity *= 1-drag*fixedDeltaTime;
		 * 200 1/5
		 * 100 1/3
		 * 25 2/3
		 * 0 1
		 * 
		
		*/
		//또한 MovePosition이 호출된 프레임에선 velocity는 아예 변화하지 않는다.
		#endregion

		

	}


}
