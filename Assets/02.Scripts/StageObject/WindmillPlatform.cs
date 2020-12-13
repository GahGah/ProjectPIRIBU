using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// goalRotation으로 돌아가는 풍차 플랫폼입니다.
/// </summary>
public class WindmillPlatform : LiftObject, ISelectable
{

    public ESelectState selectState;

    public SpriteRenderer spriteRenderer;

    [Tooltip("풍차가 돌아갈 속도 ")]
    public float windmillSpeed;
    [Tooltip("풍차의 시작 각도!")]
    public float windmillStartRotation;

    [Tooltip("정답 각도. 피리를 불면 정답 각도로 변경된 뒤 멈춘다....")]
    public float answerRotation;

    [HideInInspector, Tooltip("돌아야 할 각도(최종 목적 각도?)")]
    public float goalRotation;


    [Tooltip("정지한(해야할) 상태인가?")]
    public bool isStopped;

    [Tooltip("목적지에 도착한 상태인가?")]
    public bool isGoal;

    [Tooltip("움직이고 있는 상태인가?")]
    public bool isMoving;

    private float oldRotation;

    private Rigidbody2D rigidBody;

    private float limitDis;

    private Color debugColor;

    public void Init()
    {
        debugColor = Color.white;
        if (selectState != ESelectState.DONTSELECT)
        {
            selectState = ESelectState.DEFAULT;
        }


        isGoal = false;
        isStopped = false;
        isMoving = false;
        if (rigidBody == null)
        {
            rigidBody = gameObject.GetComponent<Rigidbody2D>();
        }

        if (windmillSpeed <= 0f)
        {
            windmillSpeed = 2f;
        }
        limitDis = 0.1f;


        if (windmillStartRotation != 0)
        {
            goalRotation = windmillStartRotation;
        }

    }
    protected override void Awake()
    {
        base.Awake();
        Init();

    }

    private void Start()
    {
        rigidBody.rotation = windmillStartRotation;
        StartCoroutine(RotateGoalRotation());


    }
    protected override void Update()
    {
        base.Update();
        HandleSelectState(selectState);
        spriteRenderer.color = debugColor;
        //Debug.Log(rigidBody.rotation);

    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        SetMovement(MovementType.SetPos, rigidBody.position);
        //수정 중인거
    }

    #region ISelectable 관련

    public void SetSelectState(ESelectState _setState)
    {
        selectState = _setState;
    }
    public void HandleSelectState(ESelectState _selectState)
    {
        if (selectState != ESelectState.DONTSELECT)
        {
            switch (selectState)
            {
                case ESelectState.DEFAULT:
                    debugColor = Color.white;
                    break;

                case ESelectState.SELECT:
                    debugColor = Color.yellow;
                    break;

                case ESelectState.CANCLE:
                    debugColor = Color.white;
                    break;

                case ESelectState.SOLVED:
                    goalRotation = answerRotation;
                    debugColor = Color.blue;
                    break;

                default:
                    break;
            }
        }
    }


    #endregion

    public IEnumerator RotateGoalRotation()
    {
        while (true)
        {
            goalRotation = ClampAngle(goalRotation);
            if (Mathf.Abs(rigidBody.rotation - goalRotation) <= 0.03f)
            {
                isGoal = true;
            }
            else
            {
                isGoal = false;
            }

            oldRotation = rigidBody.rotation;

            PlatformRotateToGoalRotation();


            yield return YieldInstructionCache.WaitForFixedUpdate;

            if (rigidBody.rotation != oldRotation)
            {
                isMoving = true;
            }
            else
            {
                isMoving = false;
            }


        }

    }

    public float ClampAngle(float _angle)
    {
        // return _angle;
        return Mathf.Clamp(_angle, -180, 180);

    }
    private void PlatformRotateToGoalRotation()
    {
        if (!isGoal)
        {
            //if (isStopped)
            //{
            //    if (selectState == ESelectState.SOLVED)
            //    {
            //        rigidBody.rotation = Mathf.MoveTowardsAngle(rigidBody.rotation, goalRotation, windmillSpeed * Time.smoothDeltaTime);
            //        return;
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}
            rigidBody.rotation = Mathf.MoveTowardsAngle(rigidBody.rotation, goalRotation, windmillSpeed * Time.smoothDeltaTime);
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
}
