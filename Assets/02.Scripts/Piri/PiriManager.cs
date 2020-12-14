using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiriManager : SingleTon<PiriManager>
{


    [Tooltip("피리 능력 횟수의 최대값입니다.")]
    public int maxPiriEnergy = 2;

    [Tooltip("피리 능력 기본 제공 횟수")]
    public int defaultPiriEnergy = 2;
    [Tooltip("현재 남은 피리 능력 사용 가능 횟수.")]
    public int currentPiriEnergy; //현재 능력 사용 가능 횟수


    [Tooltip("현재 남은 피리 능력 사용 가능 횟수를 퍼센트로 표시."), HideInInspector]
    public float currentPiriEnergyPer;
    [Tooltip("컨트롤키 꾹 누르는 시간")]
    public float ctrlInputTime;

    [Tooltip("컨트롤키 꾹 누르는 시간의...그...퍼센트를 반환."), HideInInspector]
    public float ctrlInputPer;

    [Tooltip("애를 소모해서 피리 능력을 사용한 횟수")]
    public int totaChildUseCount;
    private bool isCanAddedPressTimer;

    public bool isReadyToUse; //사용 준비 됨?

    [SerializeField]
    private float pressTimer;

    [Tooltip("Sensor레이어 제외를 위한 layerMask ")]
    int layerMask;
    private bool isChildFollow;


    [HideInInspector]
    public bool isShouldUseChild;


    protected override void Awake()
    {
        base.Awake();
        Init();
        pressTimer = 0f;

    }
    protected override void Init()
    {
        //base.Init();
        totaChildUseCount = 0;
        isCanAddedPressTimer = true;
        //defaultPiriEnergy = 3;
        //totalPiriUseCount = 0;

        ctrlInputPer = 0f;
        layerMask = (1 << LayerMask.NameToLayer("Sensor"));
        layerMask = ~layerMask;
        isChildFollow = true;


        maxPiriEnergy = 2;
        defaultPiriEnergy = 2;
        currentPiriEnergy = defaultPiriEnergy;
        isShouldUseChild = false;

        if (ctrlInputTime <= 0f)
        {
            ctrlInputTime = 1f;
        }
    }

    private void Update()
    {
        PiriProcess();
    }
    private void PiriProcess()
    {
        if (InputManager.Instance.buttonCtrl.isPressed && isCanAddedPressTimer)
        {
            //Debug.Log("isTrue");

            pressTimer += Time.smoothDeltaTime;
            ctrlInputPer = 1f * pressTimer / ctrlInputTime;
        }

        if (InputManager.Instance.buttonCtrl.wasReleasedThisFrame)
        {
            if (!isCanAddedPressTimer)
            {
                isCanAddedPressTimer = true;
            }
            pressTimer = 0f;
            ctrlInputPer = 1f * pressTimer / ctrlInputTime;
        }

        if (InputManager.Instance.buttonChildFollow.wasPressedThisFrame)
        {
            isChildFollow = !isChildFollow;
            Debug.Log("일단 아이들 따라오기 : " + isChildFollow);
            GameManager.Instance.SetChildFollow(isChildFollow);
        }

        if (pressTimer >= ctrlInputTime)
        {
            isReadyToUse = true;

            if (InputManager.Instance.buttonMouseLeft.wasPressedThisFrame)
            {


                Vector2 _tempPos = Camera.main.ScreenToWorldPoint(InputManager.Instance.GetMouseCurrentPosition());
                Debug.Log("POS :" + InputManager.Instance.GetMouseCurrentPosition());

                RaycastHit2D hit = Physics2D.Raycast(_tempPos, Vector2.zero, 0f, layerMask);


                if (hit.collider != null)
                {
                    SolveThisObject(hit.collider.gameObject);
                }
            }
        }
        else
        {
            isReadyToUse = false;
        }

        currentPiriEnergyPer = 1f * currentPiriEnergy / maxPiriEnergy;

    }
    
    /// <summary>
    /// 피리 능력 횟수를 count만큼 증가시킵니다.
    /// </summary>
    public void AddPiriEnergy(int _count)
    {
        if (currentPiriEnergy < maxPiriEnergy) //최대값보다 작으면 
        {
            currentPiriEnergy += _count;
        }
        else
        {
            //아니면 아무것도 안함. 혹시 뭔가 할지도...
        }
    }

    /// <summary>
    /// 피리 능력을 사용합니다. 성공적으로 사용했을 경우 true를 리턴합니다.
    /// </summary>
    public bool UsePiriEnergy()
    {
        if (currentPiriEnergy <= 0)
        {
            return false;
        }
        else
        {
            StartCoroutine(ProcessPiriEnergy());
            return true;
        }
    }


    public void TEST_PIRICOUNTMINUSONE()
    {
        if (currentPiriEnergy > 0)
        {
            currentPiriEnergy -= 1;
        }
        else
        {
            isShouldUseChild = true;

            if (GameManager.Instance.childs[0].enabled != false)
            {
                GameManager.Instance.childs[GameManager.Instance.childs.Count].gameObject.SetActive(false);

                GameManager.Instance.childs.Remove(GameManager.Instance.childs[GameManager.Instance.childs.Count - 1]);
                totaChildUseCount += 1;

            }

        }
    }
    public void SolveThisObject(GameObject _solveObj)
    {
        Debug.Log("Select Object name : " + _solveObj.name);


        if (_solveObj.GetComponent<PulleyPlatform>() != null)
        {
            var _test = _solveObj.GetComponent<PulleyPlatform>();
            if (_test.selectState != ESelectState.SOLVED && _test.selectState == ESelectState.DEFAULT)
            {
                if (currentPiriEnergy > 0)
                {
                    currentPiriEnergy -= 1;


                    StartPiriEffect(isShouldUseChild, _solveObj);

                    isCanAddedPressTimer = false;
                    pressTimer = 0f;

                }
                else if (GameManager.Instance.childs.Count > 0)
                {

                    if (GameManager.Instance.childs[0].enabled != false)
                    {
                        Debug.Log(GameManager.Instance.childs.Count - 1);
                        isShouldUseChild = true;


                        StartPiriEffect(isShouldUseChild, _solveObj);

                        isCanAddedPressTimer = false;
                        pressTimer = 0f;
                    }
                    else
                    {
                        isCanAddedPressTimer = false;
                        pressTimer = 0f;
                        return;
                    }


                }
                else
                {
                    isCanAddedPressTimer = false;
                    pressTimer = 0f;
                    return;
                }

                _test.selectState = ESelectState.SOLVED;




            }


        }
        else if (_solveObj.GetComponent<WindmillPlatform>() != null)
        {
            var _test = _solveObj.GetComponent<WindmillPlatform>();
            if (_test.selectState == ESelectState.DEFAULT)
            {

                if (currentPiriEnergy > 0)
                {
                    currentPiriEnergy -= 1;


                    StartPiriEffect(isShouldUseChild, _solveObj);

                    isCanAddedPressTimer = false;
                    pressTimer = 0f;

                }
                else if (GameManager.Instance.childs.Count > 0)
                {

                    if (GameManager.Instance.childs[0].enabled != false)
                    {
                        Debug.Log(GameManager.Instance.childs.Count - 1);
                        isShouldUseChild = true;


                        StartPiriEffect(isShouldUseChild, _solveObj);

                        isCanAddedPressTimer = false;
                        pressTimer = 0f;
                    }
                    else
                    {
                        isCanAddedPressTimer = false;
                        pressTimer = 0f;
                        return;
                    }


                }
                else
                {
                    isCanAddedPressTimer = false;
                    pressTimer = 0f;
                    return;
                }

                _test.selectState = ESelectState.SOLVED;

            }

        }
        else
        {
            return;
        }





        isCanAddedPressTimer = false;
        pressTimer = 0f;

    }
    private void StartPiriEffect(bool _isChildBye,GameObject _endTarget)
    {
		GameManager.Instance.UsePiri();
        ShaderManager.instance.onPostProcess();
		if (_isChildBye)
        {
            EffectManager.Instance.startEffect_VFX_PipeSkill(GameManager.Instance.childs[GameManager.Instance.childs.Count-1].gameObject, _endTarget, _isChildBye);
            GameManager.Instance.childs[GameManager.Instance.childs.Count - 1].gameObject.SetActive(false);
            GameManager.Instance.childs.Remove(GameManager.Instance.childs[GameManager.Instance.childs.Count - 1]);
        }
        else
        {
            EffectManager.Instance.startEffect_VFX_PipeSkill(GameManager.Instance.piriEffectPosition.gameObject, _endTarget, _isChildBye);
        }

    }
    private IEnumerator ProcessPiriEnergy()
    {
        while (true)
        {

            if (isReadyToUse && InputManager.Instance.buttonMouseLeft.wasPressedThisFrame // and 꾹누르는 시간이 있다면 pressTimer >시간 
                )
            {
                Debug.Log(InputManager.Instance.GetMouseCurrentPosition());

                //use 피리능력.       

                Ray ray = Camera.main.ScreenPointToRay(InputManager.Instance.GetMouseCurrentPosition());
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 10000f))
                {
                    if (hit.collider.gameObject.GetComponent<PulleyPlatform>() != null)
                    {
                        Debug.Log("Yeah~");
                    }


                }
            }
            yield return null;
        }
    }



}