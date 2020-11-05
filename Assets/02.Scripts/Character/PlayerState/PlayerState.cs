using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerState
{

    public virtual void Enter(Player _player) { }
    public virtual void Excute(Player _player) { }
    public virtual void Exit(Player _player) { }

}
