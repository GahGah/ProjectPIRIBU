using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 도르래 플랫폼.
/// </summary>
public class PulleyPlatform : LinearPlatform, ISelectable
{

    ESelectState selectState;

    public SpriteRenderer spriteRenderer;

    [Tooltip("도르래가 움직이는 속도입니다.")]
    public float pulleyDownSpeed;
    [Tooltip("도르래의 시작 위치입니다.")]
    public int pulleyStartIndex;

    [HideInInspector]
    public Vector2 routeStart;

    [Tooltip("도르래가 움직일? 뭐 그런 경로입니다. 여러개일 경우, 위(ex : 1)에서 아래(ex:3)로 놓아주시면 감사합니다.")]
    public List<GameObject> routeList;

    //[SerializeField]
    //private bool isOneRoute; // 경로가 하나 뿐인가?
    [Tooltip("움직일 경로의 인덱스. -99일 경우 시작위치.")]
    public int currentRouteIndex;


    [Tooltip("움직여도 되는 상태인가? (발판이 눌렸을 때나 레버가 당겨졌을 때 true여야함)")]
    public bool isCanMove;

    [Tooltip("움직이고 있는 상태인가?")]
    public bool isMoving;

    private Vector2 oldPosition;


    private Rigidbody2D rigidBody;

    private float limitDis;

    public void Init()
    {

        selectState = ESelectState.DEFAULT;

        isCanMove = true;
        isMoving = false;

        if (rigidBody == null)
        {
            rigidBody = gameObject.GetComponent<Rigidbody2D>();
        }

        if (pulleyDownSpeed <= 0f)
        {
            pulleyDownSpeed = 2f;
        }
        limitDis = 0.1f;


        routeStart = rigidBody.position;

        if (pulleyStartIndex != 0) // 시작ㅇ ㅟ치가 정해져있으면???
        {
            gameObject.transform.position = routeList[pulleyStartIndex].transform.position; //위치 설정 

            currentRouteIndex = pulleyStartIndex;

        }
        else
        {
            gameObject.transform.position = routeList[0].transform.position;
            currentRouteIndex = 0;
        }




        //rigidBody.position = routeStart.transform.position;
    }
    protected override void Awake()
    {
        base.Awake();
        Init();

    }

    private void Start()
    {
        StartCoroutine(MovePlatformThisRoute());
        ClampRouteIndex(7);
    }
    protected override void Update()
    {
        base.Update();

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        //수정 중인거
    }

    #region ISelectable 관련

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


    #endregion

    public IEnumerator MovePlatformThisRoute()
    {
        while (true)
        {
            currentRouteIndex = ClampRouteIndex(currentRouteIndex);
            oldPosition = rigidBody.position;
            if (isCanMove) //레버가 간섭하고 있지 않으면
            {
                if (GameManager.Instance.isDebugMode)
                {
                    spriteRenderer.color = Color.blue;
                }

                PlatformMovePosition(routeList[ClampRouteIndex(currentRouteIndex)].transform.position);

            }
            else
            {
                if (GameManager.Instance.isDebugMode)
                {
                    spriteRenderer.color = Color.red;
                }
                currentRouteIndex = 0;
                PlatformMovePosition(routeList[0].transform.position);
               

            }

            yield return YieldInstructionCache.WaitForFixedUpdate;

            if (rigidBody.position != oldPosition)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }
        }


        
    }

    public int ClampRouteIndex(int _index)
    {
        Debug.Log(Mathf.Clamp(_index, 0, routeList.Count - 1));
        return Mathf.Clamp(_index, 0, routeList.Count - 1);

    }
    private void PlatformMovePosition(Vector2 _movePosition)
    {
        SetMovement(MovementType.SetPos, Vector2.MoveTowards(rigidBody.position, _movePosition, pulleyDownSpeed * Time.smoothDeltaTime));

        //rigidBody.MovePosition(Vector2.MoveTowards(rigidBody.position, _movePosition, pulleyDownSpeed * Time.smoothDeltaTime));
        //rigidBody.position = Vector2.MoveTowards(rigidBody.position, _movePosition, pulleyDownSpeed * Time.smoothDeltaTime);
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
