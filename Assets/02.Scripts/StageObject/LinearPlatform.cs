using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// LinearPlatform은 크기 1유닛 짜리 기본 Box의 윗면을 지면으로 하는 플랫폼이다.
/// </summary>
public class LinearPlatform : LiftObject {

	public bool isOneWay = true;

	protected override void Awake() {
		base.Awake();

		gameObject.tag = "LinearPlatform";
		if (isOneWay) {
			gameObject.layer = LayerMask.NameToLayer("OneWayPlatform");
		} else {
			gameObject.layer = LayerMask.NameToLayer("Ground");
		}
		
	}

	protected virtual void Update() {}

	public bool GetIsOneWayIgnore(Vector3 charPos) {

		//Vector3 center = currPos + currNormal * currScale.y * 0.5f;//지형 선분의 중앙점
		Vector3 center = currPos;//중앙점
		bool isOneWayIgnore = true;//충돌연산 Off

		//캐릭터가 표면 위에 있다면 캐릭터는 충돌해야한다.
		if (Vector3.Dot(currNormal, charPos - center) >= 0) {
			//Debug.DrawLine(currPos, currPos + currNormal, Color.red);
			//Debug.DrawLine(currPos, currPos + (charPos - center), Color.blue);
			isOneWayIgnore = false;
		}

		return isOneWayIgnore;
	}

	public void DrawPlatformSegment() {
		Vector3 up, right;
		up = transform.up;
		right = transform.right;

		Vector3 leftTop, rightTop;
		leftTop = currPos + up * currScale.y * 0.5f - right * currScale.x * 0.5f;
		leftTop.z = 0;
		rightTop = leftTop + right * currScale.x;
		Debug.DrawLine(leftTop, rightTop, Color.green);
	}
}
