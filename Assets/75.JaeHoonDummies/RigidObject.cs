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
		/*
		// 1/fixedDeltaTime는 초당 물리연산 횟수이다.
		float mult = rigid.mass / Time.fixedDeltaTime / Time.fixedDeltaTime ;
		float moveMount = 15;

		//AddForce는 왼쪽 위를 향한다.
		rigid.AddForce((Vector2.left + Vector2.up)* moveMount * mult);

		//MovePosition은 오른쪽 위를 향한다.
		rigid.MovePosition(transform.position+ (Vector3.right + Vector3.up) * moveMount);

		//두 연산이 합쳐져 결과적으로 한프레임당 Vector3.up*moveMount만큼 이동한다.
		*/
		#endregion

		Vector3 pos = transform.position;
		float moveMount = 1f;
		rigid.MovePosition(pos + Vector3.right * moveMount);
		rigid.MovePosition(pos - Vector3.right * moveMount);
		//결과 : 마지막에 호출된 MovePosition만 적용되어 프레임당 1씩 왼쪽으로 이동한다.

	}
	Vector3 currPos;


}
