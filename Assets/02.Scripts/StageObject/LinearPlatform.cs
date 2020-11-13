using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// LinearPlatform은 크기 1유닛 짜리 기본 Box의 윗면을 지면으로 하는 플랫폼이다.
/// </summary>
public class LinearPlatform : MonoBehaviour {
	public bool isOneWay = true;
	[HideInInspector]
	public Vector3
		prePos, currPos,
		preScale, currScale,
		preAngle, currAngle,
		preNormal, currNormal;

	private void Awake() {
		gameObject.tag = "LinearPlatform";
		if (isOneWay) {
			gameObject.layer = LayerMask.NameToLayer("OneWayPlatform");
		} else {
			gameObject.layer = LayerMask.NameToLayer("Ground");
		}
		UpdateCurrTransforms();
		UpdatePreTransforms();
	}

	void FixedUpdate() {
		UpdatePreTransforms();

		Vector3 up, right;
		up = transform.up;
		right = transform.right;

		Vector3 leftTop, rightTop;
		leftTop = currPos + up * currScale.y * 0.5f - right * currScale.x * 0.5f;
		leftTop.z = 0;
		rightTop = leftTop + right * currScale.x;
		Debug.DrawLine(leftTop, rightTop, Color.red);

		UpdateCurrTransforms();
	}

	//1프레임 전 트랜스폼 업데이트
	private void UpdatePreTransforms() {
		prePos = currPos;
		preScale = currScale;
		preAngle = currAngle;
		preNormal = currNormal;
	}

	//현재프레임 트랜스폼 업데이트
	private void UpdateCurrTransforms() {
		currPos = transform.position;
		currAngle = transform.eulerAngles;
		currScale = transform.localScale;
		currNormal = transform.up;
	}

	public bool GetIsOneWayIgnore(Vector3 charPos, Vector3 charVelocity) {

		Vector3 center = currPos + currNormal * currScale.y * 0.5f;//지형 선분의 중앙점
		bool isOneWayIgnore = true;//충돌연산 Off

		if (Vector3.Dot(currNormal, charPos - center) >= 0//캐릭터가 표면 위에 있다면
			//&& Vector3.Dot(charVelocity, currNormal) <= 0//캐릭터 속도는 표면노말과...
			) {

			Debug.DrawLine(currPos, currPos + currNormal, Color.red);
			Debug.DrawLine(currPos, currPos + (charPos - center), Color.blue);
			isOneWayIgnore = false;//캐릭터는 충돌 해야한다.
		}

		return isOneWayIgnore;
	}

	public Vector3 GetLiftPosition(Vector3 charPos) {
		return Vector3.zero;
	}
}
