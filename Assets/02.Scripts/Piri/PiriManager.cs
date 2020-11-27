using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiriManager : SingleTon<PiriManager>
{
    [Tooltip("피리 능력 기본 제공 횟수")]
    public int defaultPiriEnergy = 3;
    [Tooltip("현재 남은 피리 능력 사용 가능 횟수.")]
    public int currentPiriEnergy; //현재 능력 사용 가능 횟수

    [Tooltip("피리 능력 횟수의 최대값입니다.")]
    public int maxPiriEnergy = 10;
    // Start is called before the first frame update

    [Tooltip("피리 능력 총 사용 횟수")]
    private int totalPiriUseCount;

    [Tooltip("컨트롤키 꾹 누르는 시간")]
    public float ctrlInputTime;
    private bool isReadyToUse; //사용 준비 됨?

    float pressTimer;
    public int getTotalPiriUseCount
    {
        get
        {
            return totalPiriUseCount;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        //Init();
        pressTimer = 0f;

    }

    //private void Start()
    //{
    //   // StartCoroutine(ProcessPiriEnergy());
    //}
    private void Update()
    {
        if (InputManager.instance.buttonCtrl.isPressed)
        {
            //Debug.Log("isTrue");
            isReadyToUse = true;
            pressTimer += Time.smoothDeltaTime;
        }
        else
        {
            isReadyToUse = false;
            pressTimer = 0f;
        }

        if (pressTimer>=ctrlInputTime)
        {
            Debug.Log("OK");
            if (InputManager.instance.buttonMouseLeft.wasPressedThisFrame)
            {
                Ray ray = Camera.main.ScreenPointToRay(InputManager.instance.GetMouseCurrentPosition());
                Debug.Log("POS :" + InputManager.instance.GetMouseCurrentPosition());
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 10000f))
                {
                    if (hit.collider.GetComponent<PulleyPlatform>() != null)
                    {
                        Debug.Log("PulleyPlatform");
                    }

                }
            }


        }

    }
    protected override void Init()
    {
        base.Init();
        defaultPiriEnergy = 3;
        totalPiriUseCount = 0;
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
    private IEnumerator ProcessPiriEnergy()
    {
        while (true)
        {

            if (isReadyToUse && InputManager.instance.buttonMouseLeft.wasPressedThisFrame // and 꾹누르는 시간이 있다면 pressTimer >시간 
                )
            {
                Debug.Log(InputManager.instance.GetMouseCurrentPosition());

                //use 피리능력.       

                Ray ray = Camera.main.ScreenPointToRay(InputManager.instance.GetMouseCurrentPosition());
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 10000f))
                {
                    if (hit.collider.gameObject.GetComponent<PulleyPlatform>() != null)
                    {
                        Debug.Log("Yeah~");
                    }
                    /*
                     * if hit.콜라이더.컴페어태그(무언가의...그...가능한 태그들)
                     * { 능력사용.}
                     * 
                     */


                }
            }
            yield return null;
        }
    }
}