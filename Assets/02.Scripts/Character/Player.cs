using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//주인공, 우리가 조작하는 플레이어인 피리부
public class Player : Character
{
    public string playerName = "피리부"; // 피리부
    public PlayerState currentState; //현재 상태

    private IEnumerator currentStateCoroutine; //현재, 돌아가고 있는 state의 코루틴.

    private void Awake()
    {

    }


    private void Update()
    {
        if (currentState.GetType() == typeof(PlayerDoMove)) //현재 상태가 두무브라면
        {
            Debug.Log("DoMove");
        }
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
        if (CurrentStateExcute() !=null && currentStateCoroutine !=CurrentStateExcute())
        {
            currentStateCoroutine = CurrentStateExcute();
        }
        StartCoroutine(currentStateCoroutine);
       
    }

    public void StopCurrentStateExcute() //  CurrentStateExcute()를 스톱
    {
        if (CurrentStateExcute() !=null && currentStateCoroutine != CurrentStateExcute())
        {
            currentStateCoroutine = CurrentStateExcute();
        }
        StopCoroutine(currentStateCoroutine);
    }


    IEnumerator CurrentStateExcute()
    {
        while (true)
        {
            currentState.Excute(this);
            yield return null;
        }
    }
    #endregion


    

}
