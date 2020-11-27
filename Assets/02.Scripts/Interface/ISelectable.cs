using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//피리로 선택하여 상호작용할 수 있는 오브젝트에 사용되는 interface
public interface ISelectable
{

   void HandleSelectState(ESelectState _selectState);
}
/// <summary>
/// DEFAULT = 0, SELECT, CANCLE
/// </summary>
public enum ESelectState
{
    DEFAULT = 0,
    SELECT,
    CANCLE
}