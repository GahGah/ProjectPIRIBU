using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 도르래 플랫폼.
/// </summary>
public class PulleyPlatform : LiftObject, ISelectable
{

    ESelectState selectState;

    [Tooltip("도르래가 움직일? 뭐 그런 경로입니다. 여러개일 경우, 위(ex : 1)에서 아래(ex:3)로 놓아주시면 감사합니다.")]
    public List<Transform> routeList = null; // 경로 리스트.
    [SerializeField]
    private bool isOneRoute; // 경로가 하나 뿐인가?

    
    protected override void Awake()
    {
        base.Awake();
        if (routeList.Count == 1)
        {
            isOneRoute = true;
        }
        else
        {
            isOneRoute = false;
        }

        selectState = ESelectState.DEFAULT;
    }


    public void SetSelectState(ESelectState _setState)
    {
        selectState = _setState;
    }
    public void HandleSelectState(ESelectState _selectState)
    {
        switch (selectState)
        {
            case ESelectState.DEFAULT:
                break;

            case ESelectState.SELECT:
                break;

            case ESelectState.CANCLE:
                break;

            default:
                break;
        }
    }
    //// Start is called before the first frame update
    //void Start()
    //{
        
    //}

    //// Update is called once per frame
    //void Update()
    //{
        
    //}
}
