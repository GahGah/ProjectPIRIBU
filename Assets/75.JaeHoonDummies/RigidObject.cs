using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RigidObject : MonoBehaviour
{
	Rigidbody2D rigid;
	Vector3 firstPos;
    void Start()
    {
		rigid = GetComponent<Rigidbody2D>();
		firstPos = transform.position;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
		Debug.DrawLine(transform.position,transform.position+Vector3.up);
		rigid.MovePosition(transform.position + Vector3.right);

	}


}
