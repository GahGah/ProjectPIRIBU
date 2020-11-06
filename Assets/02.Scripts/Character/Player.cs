using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//주인공, 우리가 조작하는 플레이어인 피리부
public class Player : Character
{
    // 캐싱해서 사용. new 쓰는 것 보다는 도움이 될 것 같아서...
    public static readonly WaitForFixedUpdate WaitForFixedUpdate = new WaitForFixedUpdate();

    public string playerName = "피리부"; // 피리부
    public PlayerState currentState = null; //현재 상태
    public Rigidbody2D playerRig = null;
    public float playerSpeed = 10f;
    public float playerJumpPower = 5f;

    private IEnumerator currentStateCoroutine = null; //현재, 돌아가고 있는 state의 코루틴.
    private IEnumerator currentCoroutine = null; //Start & Stop을 원활하게 하기 위함. 별 의미 없음. 
    public void Init()
    {
        if (playerRig == null)
        {
            playerRig = GetComponent<Rigidbody2D>();
        }
    }
    private void Awake()
    {
        Init();
    }

    #region PlayerState 관련---
    public void ChangeState(PlayerState _newState)
    {
        if (currentState != null)
        {
            currentState.Exit(this);
        }
        currentState = _newState;
        currentState.Enter(this);
    }

    public void StartCurrentStateExcute() //  CurrentStateExcute()를 시작함
    {
        if (CurrentStateExcute() != null && currentStateCoroutine != CurrentStateExcute())
        {
            currentStateCoroutine = CurrentStateExcute();
        }
        StartCoroutine(currentStateCoroutine);

    }

    public void StopCurrentStateExcute() //  CurrentStateExcute()를 스톱
    {
        if (CurrentStateExcute() != null && currentStateCoroutine != CurrentStateExcute())
        {
            currentStateCoroutine = CurrentStateExcute();
        }
        StopCoroutine(currentStateCoroutine);
    }


    private IEnumerator CurrentStateExcute()
    {
        while (true)
        {
            currentState.Excute(this);
            yield return null;
        }
    }
    #endregion



    public void StartMove() // 외부에서 사용하기 위해
    {
        currentCoroutine = Move();
        StartCoroutine(Move());
    }
    public void StopMove() //외부에서 사용하기 위해
    {
        currentCoroutine = Move();
        StopCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (true)
        {
            playerRig.velocity = new Vector2(GameManager.Instance.inputManager.moveVal.x, playerRig.velocity.y);
            if (GameManager.Instance.inputManager.isJumpKeyPressed && playerRig.velocity.y == 0f) //점프 키 입력이 되었을 경우
            {
                playerRig.AddForce(new Vector2(playerRig.velocity.x, playerJumpPower), ForceMode2D.Impulse);
            }

            yield return WaitForFixedUpdate;

        }

    }

}
