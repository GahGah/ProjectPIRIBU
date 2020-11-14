using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingleTon<GameManager>
{
    /* 
     * "기능 플랫폼, 그래픽 리소스"들의 Start 함수에서 isDebugMode 변수 체크.
     * if: isDebugMode == true 일 시,
     * [기능 플랫폼].Renderer.enabled = true; [그래픽 리소스].Renderer.enabled = false;
     * else:
     * [기능 플랫폼].Renderer.enabled = false; [그래픽 리소스].Rnderer.enabled = true;
     */

    public bool isDebugMode = false;   

    public float timer = 0f;

    public InputManager inputManager;

	protected override void Awake() {
		base.Awake();//여기서 Init한다.
	}

	protected override void Init()
    {
		base.Init();
		inputManager = InputManager.Instance;
    }


	private void Update() {
		//ESC키 누르면 임의 리셋
		if (inputManager.buttonPause.isPressed) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}


	IEnumerator GameProcessTimer() // 인게임 내부에서 돌아가는 실제시간 타이머 (테스트용)
    {
        timer = 0f;

        while (true)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
    }


}
