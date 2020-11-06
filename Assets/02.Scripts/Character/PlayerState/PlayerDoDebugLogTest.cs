using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoDebugLogTest : PlayerState
{
    public override void Enter(Player _player)
    {
        _player.StartCurrentStateExcute();
        Debug.Log("TestEnter");
    }
    public override void Excute(Player _player)
    {
        //Debug.Log("TestExcute");
    }
    public override void Exit(Player _player)
    {
        _player.StopCurrentStateExcute();
        Debug.Log("TestExit");
    }
}
