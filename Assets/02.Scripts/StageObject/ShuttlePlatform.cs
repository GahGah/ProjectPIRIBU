using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShuttlePlatform : MonoBehaviour
{
	Vector3 firstPos;
	Vector3 prePos, currPos;
	float fixedUpdatePerSec;
	private void Awake() {
		firstPos = transform.position;
		currPos = firstPos;
		prePos = firstPos;
		fixedUpdatePerSec = 1 / Time.fixedDeltaTime;
	}
	private void FixedUpdate() {
		
		prePos = currPos;
		currPos =
			firstPos + 
			Vector3.left * Mathf.Sin(Time.time*2)*2f
			+ Vector3.up * Mathf.Cos(Time.time*2)*0f;

		GetComponent<Rigidbody2D>().velocity = (currPos - prePos)* fixedUpdatePerSec;
		transform.position = currPos;
	}
}
