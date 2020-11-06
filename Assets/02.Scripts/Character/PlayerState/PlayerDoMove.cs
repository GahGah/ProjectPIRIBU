using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어가 좌, 우로 움직이는 상태.
public class PlayerDoMove : PlayerState
{
    public override void Enter(Player _player)
    {
        _player.StartCurrentStateExcute();
        _player.StartMove();
    }
    public override void Excute(Player _player)
    {
    }
    public override void Exit(Player _player)
    {
        _player.StopCurrentStateExcute();
        _player.StopMove();
    }
}
