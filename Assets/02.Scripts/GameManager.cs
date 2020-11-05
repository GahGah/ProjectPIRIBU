using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float timer = 0f;
    public static GameManager Instance;

    public Player player;
    private void Awake()
    {
        if (Instance ==null)
        {
            Instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        StartCoroutine(GameProcessTimeEvent());
    }

    IEnumerator GameProcessTimeEvent() //인게임의 시간당 이벤트 (테스트용) 
    {
        StartCoroutine(GameProcessTimer());
        float eventTime = 5f;

        player.ChangeState(new PlayerDoDebugLogTest());

        while (timer < eventTime)
        {
            yield return null;
        }
        player.ChangeState(new PlayerDoMove());
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
