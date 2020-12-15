using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CreditManager : MonoBehaviour
{
	public Animator anim;
	public bool isEndCredit;
	public string nextScene;
	float targetSpeed,currSpeed;


	private void Awake() {
		isEndCredit = false;
		targetSpeed = 1;
		currSpeed = 1;
	}

	// Update is called once per frame
	void Update()
    {
		if (!isEndCredit) {
			if (InputManager.Instance.buttonMouseLeft.isPressed || Keyboard.current.anyKey.isPressed) {
				targetSpeed = 7;
			} else { targetSpeed = 1; }

			currSpeed = Mathf.Lerp(currSpeed, targetSpeed, Time.deltaTime * 10f);
			anim.speed = currSpeed;
		} else {
			anim.speed = 0;
		}
	}

	public void EndCredit() {
		isEndCredit = true;
		SceneChanger.Instance.LoadScene(nextScene);
	}
}
