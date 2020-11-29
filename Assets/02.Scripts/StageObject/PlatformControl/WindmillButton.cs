using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindmillButton : PlatformController
{
    public SpriteRenderer spriteRenderer;

    [Tooltip("버튼을 한 번 누르면 다시 누를 수 있는 시간(초)")]
    public float buttonDelayTime;

    [Tooltip("buttonDelayTime에 상관없이,\n담당 풍차(?)가 할당한 각도만큼 기울어지기 전 까지 버튼을 누른 상태로 해둡니다.")]
    public bool isDelayToGoal;

    [Tooltip("컨트롤할 풍차 플랫폼의 리스트")]
    public List<WindmillPlatform> windmillPlatformList;

    [Tooltip("버튼을 누를 때마다 더해지는 각도. \n일단 컨트롤오브젝트리스트당 하나씩만 넣는 것으로 따졌음~)")]
    public float[] goalAngleList;

    [Header("[임시] 버튼의 온/오프 이미지. ")]
    public Sprite buttonOnImage;
    public Sprite buttonOffImage;

    WaitForSeconds buttonDelaySeconds;
    public void Init()
    {
        buttonDelaySeconds = new WaitForSeconds(buttonDelayTime);
        isActive = false;

        if (windmillPlatformList.Count == 1)
        {
            isControlOneObject = true;
        }
        else
        {
            isControlOneObject = false;
        }

        if (rangeDistance <= 0f)
        {
            rangeDistance = 1f;
        }
        isCanActive = true;
    }
    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        StartCoroutine(ProcessButton());
    }
    private void Update()
    {
    }


    public IEnumerator ProcessButton()
    {
        while (true)
        {
            if (InputManager.Instance.buttonCatch.wasPressedThisFrame && isCanActive)
            {
                if (Vector2.Distance(GameManager.Instance.hero.transform.position, gameObject.transform.position) <= rangeDistance)
                {
                    SetIsActive(true);
                    spriteRenderer.sprite = buttonOnImage;
                    //Debug.Log("toggle! : " + isActive);


                    spriteRenderer.sprite = buttonOnImage;
                    if (isControlOneObject) //컨트롤 할게 한 오브젝트 뿐일경우
                    {
                        if (windmillPlatformList[0].IsCanControl() == true)
                        {

                            windmillPlatformList[0].goalRotation += goalAngleList[0]; //할당량만큼 더하기.
                        }

                    }
                    else //한개가 아닐 경우...
                    {
                        for (int i = 0; i < windmillPlatformList.Count; i++)
                        {
                            if (windmillPlatformList[i].IsCanControl() == true) //조종할 수 있는 상태라면
                            {
                                windmillPlatformList[i].goalRotation += goalAngleList[i];

                            }
                        }
                    }
                    yield return YieldInstructionCache.WaitForFixedUpdate;
                    if (isDelayToGoal) //딜레이가 다 기울어지기 전 까지냐?
                    {
                        if (isControlOneObject) //컨트롤 할게 한 오브젝트 뿐일경우
                        {
                            while (!windmillPlatformList[0].isGoal)
                            {
                                yield return YieldInstructionCache.WaitForFixedUpdate;
                            }
                        }
                        else //한개가 아닐 경우...
                        {
                            while (true)
                            {
                                bool isEnd = true;
                                for (int i = 0; i < windmillPlatformList.Count; i++)
                                {
                                    if (windmillPlatformList[i].IsCanControl() == true) //조종할 수 있는 상태라면
                                    {
                                        if (windmillPlatformList[i].isGoal==false)
                                        {
                                            isEnd = false;
                                            break;
                                        }
     
                                    }
                                }

                                if (isEnd)
                                {
                                    break;
                                }
                                else
                                {
                                    yield return YieldInstructionCache.WaitForFixedUpdate;
                                }
                            }
                        }
                    }
                    else
                    {
                        yield return buttonDelaySeconds;
                    }

                    SetIsActive(false);
                    spriteRenderer.sprite = buttonOffImage;
                }
            }
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(gameObject.transform.position, rangeDistance);
    }
}
