using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBox : InteractionObject
{
	public Rigidbody2D playerRigid;
	public HingeJoint2D joint;
	float sqrDist = 10 * 10;

    // Update is called once per frame
    void Update()
    {
		//캐릭터 근처 있을시
		if (Vector3.SqrMagnitude((Vector3)playerRigid.position - transform.position) < sqrDist) { 

			if (InputManager.Instance.buttonCatch.isPressed) {
				if (joint == null) {
					joint = gameObject.AddComponent<HingeJoint2D>();
					joint.autoConfigureConnectedAnchor = true;
					joint.connectedBody = playerRigid;
					//joint.breakForce = 100;
				}
			}

			else {
				//삭제
				BreakJoint();
			}
		} else {
			BreakJoint();
		}
    }

	void BreakJoint() {
		if (joint) {
			joint.breakForce = 0;//알아서 컴포넌트 삭제됨
			joint = null;
		}
	}
}
