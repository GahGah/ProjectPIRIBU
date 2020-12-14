using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 도르래 플랫폼.
/// </summary>
public class PulleyPlatform : LinearPlatform, ISelectable
{

    public ESelectState selectState;

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
    [Tooltip("갈(가 있는?) 경로 번호.")]
    public int currentRouteIndex;

    [Tooltip("정답 경로의 번호. 피리를 사용하면 해당 경로로 이동한 뒤 멈춥니다.")]
    public int answerRouteIndex;

    [Tooltip("움직여도 되는 상태인가? (발판이 눌렸을 때나 레버가 당겨졌을 때 true여야함)")]
    public bool isCanMove;


    [Tooltip("정지한(해야할) 상태인가?")]
    public bool isStopped;

    [Tooltip("지정 경로로 도착하였는가?")]
    public bool isArrive;

    [Tooltip("움직이고 있는 상태인가?")]
    public bool isMoving;

    private Vector2 oldPosition;

    private Rigidbody2D rigidBody;

    private float limitDis;

    private Color debugColor;

    public GameObject effectObject;
    public void Init()
    {
        effectObject.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.black);

        debugColor = Color.white;
        if (selectState != ESelectState.DONTSELECT)
        {
            selectState = ESelectState.DEFAULT;
        }

        isArrive = false;
        isStopped = false;
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

        if (pulleyStartIndex >= 0) // 시작ㅇ ㅟ치가 정해져있으면???
        {
            currentRouteIndex = ClampRouteIndex(pulleyStartIndex);
            gameObject.transform.position = routeList[currentRouteIndex].transform.position; //위치 설정 

        }
        else if (pulleyStartIndex == 0)
        {

            currentRouteIndex = 0;
            gameObject.transform.position = routeList[0].transform.position;
        }
        else if (pulleyStartIndex < 0)
        {

            currentRouteIndex = 0;
            gameObject.transform.position = routeList[0].transform.position;
        }
        else
        {

        }

    }
    protected override void Awake()
    {
        base.Awake();
        Init();

    }

    private void Start()
    {
        rigidBody.position = routeList[currentRouteIndex].transform.position;
        StartCoroutine(MovePlatformThisRoute());
        ClampRouteIndex(7);

        foreach (var item in routeList)
        {
            item.GetComponent<SpriteRenderer>().enabled = false;
        }

    }
    protected override void Update()
    {
        base.Update();


        HandleSelectState(selectState);
        spriteRenderer.color = debugColor;

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
        if (selectState!=ESelectState.DONTSELECT)
        {
            switch (selectState)
            {


                case ESelectState.SELECT:
                    ShaderManager.Instance.changeBokeColor(effectObject, new Color32(191, 98, 28, 255));
                    // debugColor = Color.yellow;
                    break;

                case ESelectState.DEFAULT:
                    debugColor = Color.white;
                    if (PiriManager.Instance.hit.collider.gameObject == this.gameObject)
                    {
                        ShaderManager.Instance.changeBokeColor(effectObject, new Color32(191, 98, 28, 255));
                    }
                    else
                    {
                        ShaderManager.Instance.changeBokeColor(effectObject, new Color32(191, 98, 28, 255));
                    }
                    break;

                case ESelectState.CANCLE:
                    debugColor = Color.white;
                    break;

                case ESelectState.SOLVED:
                    // debugColor = Color.blue;
                   
                    isCanMove = true;
                    isStopped = false;
                    currentRouteIndex = answerRouteIndex;
                    break;

                default:
                    break;
            }
        }
     
    }


    #endregion

    public IEnumerator MovePlatformThisRoute()
    {
        while (true)
        {
            if (Mathf.Abs(Vector2.Distance(gameObject.transform.position, routeList[ClampRouteIndex(currentRouteIndex)].transform.position)) <= 0.03f)
            {
                isArrive = true;
            }
            else
            {
                isArrive = false;
            }
            currentRouteIndex = ClampRouteIndex(currentRouteIndex);
            oldPosition = rigidBody.position;
            if (isCanMove) //레버가 간섭하고 있지 않으면
            {
                if (!isStopped)// 정지해야할 상태라면
                {                    //지정된 경로로 이동.
                    PlatformMovePosition(routeList[ClampRouteIndex(currentRouteIndex)].transform.position);
                    //움직이지 않는다.
                }

            }
            else
            {
                if (!isStopped)
                {
                    currentRouteIndex = 0;
                    PlatformMovePosition(routeList[0].transform.position);
                }


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
        //Debug.Log(Mathf.Clamp(_index, 0, routeList.Count - 1));
        return Mathf.Clamp(_index, 0, routeList.Count - 1);

    }
    private void PlatformMovePosition(Vector2 _movePosition)
    {
        if (!isStopped)
        {
            SetMovement(MovementType.SetPos, Vector2.MoveTowards(rigidBody.position, _movePosition, pulleyDownSpeed * Time.smoothDeltaTime));
        }
       
        //rigidBody.MovePosition(Vector2.MoveTowards(rigidBody.position, _movePosition, pulleyDownSpeed * Time.smoothDeltaTime));
        //rigidBody.position = Vector2.MoveTowards(rigidBody.position, _movePosition, pulleyDownSpeed * Time.smoothDeltaTime);
    }

    /// <summary>
    /// 레버나 발판으로 조종할 수 있는 상태라면 true를 리턴~
    /// </summary>
    /// <returns></returns>
    public bool IsCanControl()
    {
        if (selectState == ESelectState.SOLVED)
        {
            return false;
        }
        else
        {
            return true;
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
