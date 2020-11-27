using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 레버 슈퍼 클래스. 
/// </summary>
public class PlatformController : MonoBehaviour
{
    [Tooltip("작동되어있는 상태인가?")]
    public bool isActive;


    [Tooltip("컨트롤 할 오브젝트가 하나뿐인가?")]
    public bool isControlOneObject;

    [Tooltip("레버 작동 판정범위")]
    public float rangeDistance;


    [Tooltip("레버를 작동시킬 수 있는 상태인가?")]
    public bool isCanActive;
    /// <summary>
    /// isActive = !isActive
    /// </summary>
    public void ToggleIsActive()
    {
        isActive = !isActive;
    }
    public void SetIsActive(bool _isActive)
    {
        isActive = _isActive;
    }

}
