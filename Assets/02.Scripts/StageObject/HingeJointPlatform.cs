using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(HingeJoint2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class HingeJointPlatform : MonoBehaviour
{
	private HingeJoint2D hinge;
	private Rigidbody2D rigid;

	[Range(-45, 45)]
	[FormerlySerializedAs("원래 각도")]
	public float targetAngle = 0;//돌아올 앵글

	[Range(0.1f,20)]
	[FormerlySerializedAs("탄성력")]
	public float recoveryPower = 2f;

	[Range(1, 100)]
	[FormerlySerializedAs("시소 중량")]
	public float weight = 2f;

	private void Start() {
		hinge = GetComponent<HingeJoint2D>();
		rigid = GetComponent<Rigidbody2D>();
	}
	void Update()
    {
		rigid.mass = weight;

		JointMotor2D motor = hinge.motor;
		float currentAngle = getLoopedAngle(transform.eulerAngles.z);
		motor.motorSpeed = -(targetAngle - currentAngle)* recoveryPower;
		hinge.motor = motor;
	}

	float getLoopedAngle(float angle) {
		while(angle > 180) { angle -= 360; }
		while (angle < -180) { angle += 360; }
		return angle;
	}
}
