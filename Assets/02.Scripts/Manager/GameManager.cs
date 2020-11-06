using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingleTon<GameManager>
{
    public float timer = 0f;

    public InputManager inputManager;
    private void Init()
    {
        if (inputManager==null)
        {
            inputManager = GetComponent<InputManager>();
        }

    }
    protected override void Awake()
    {
		base.Awake();
		Init();
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
