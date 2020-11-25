using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(HingeJoint2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class HingeJointPlatform : LinearPlatform {
	private HingeJoint2D hinge;

	[Range(-45, 45)]
	[FormerlySerializedAs("원래 각도")]
	public float targetAngle = 0;//돌아올 앵글

	[Range(0.1f,100)]
	[FormerlySerializedAs("탄성력")]
	public float recoveryPower = 2f;

	[Range(1, 300)]
	[FormerlySerializedAs("시소 중량")]
	public float weight = 2f;

	protected override void Awake() {
		base.Awake();
		hinge = GetComponent<HingeJoint2D>();
	}
	protected override void Update()
	{
		base.Update();
		rigid.mass = weight;

		JointMotor2D motor = hinge.motor;
		float currentAngle = getLoopedAngle(transform.eulerAngles.z);
		motor.motorSpeed = -(targetAngle - currentAngle)* recoveryPower;
		motor.maxMotorTorque = Mathf.Pow(recoveryPower,1.2f);
		hinge.motor = motor;
	}

	float getLoopedAngle(float angle) {
		while(angle > 180) { angle -= 360; }
		while (angle < -180) { angle += 360; }
		return angle;
	}
}
