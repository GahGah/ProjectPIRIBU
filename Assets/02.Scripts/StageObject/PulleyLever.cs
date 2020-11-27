using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulleyLever : PlatformController
{
    public SpriteRenderer spriteRenderer;

    [Tooltip("컨트롤할 도르래 플랫폼의 리스트")]
    public List<PulleyPlatform> pulleyPlatformList;

    [Tooltip("이동할 경로의 인덱스(PulleyPlatform의 routeList Index. \n일단 컨트롤오브젝트리스트당 하나씩만 넣는 것으로 따졌음~)")]
    public int[] pulleyRouteList;

    [Header("[임시] 레버의 온/오프 이미지. ")]
    public Sprite leverOnImage;
    public Sprite leverOffImage;


    public void Init()
    {
        isActive = false;

        if (pulleyPlatformList.Count == 1)
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

    //private void Start()
    //{
    //    //isActive = true;
    //}


    private void Update()
    {

        if (InputManager.Instance.buttonCatch.wasPressedThisFrame)
        {
            if (!isCanActive)
            {
                return;
            }
            if (!(Vector2.Distance(GameManager.Instance.hero.transform.position, gameObject.transform.position) <= rangeDistance))
            {
                return;
            }
            //for (int i = 0; i < pulleyPlatformList.Count; i++)
            //{
            //    if (pulleyPlatformList[i].isMoving == true)
            //    {

            //        return;
            //    }

            //}
            ToggleIsActive();
            Debug.Log("toggle! : " + isActive);

            if (isActive == true) //레버가 온! 되었다면?
            {
                spriteRenderer.sprite = leverOnImage;
                if (isControlOneObject) //컨트롤 할게 한 오브젝트 뿐일경우
                {
                    pulleyPlatformList[0].isCanMove = true;

                    pulleyPlatformList[0].currentRouteIndex += pulleyRouteList[0]; //할당량만큼 더하기.

                }
                else //아니면...
                {
                    for (int i = 0; i < pulleyPlatformList.Count; i++)
                    {
                        pulleyPlatformList[i].isCanMove = true;

                        pulleyPlatformList[i].currentRouteIndex += pulleyRouteList[i];
                        Debug.Log(i + "번째 도르래 인덱스 : " + pulleyPlatformList[i].currentRouteIndex);
                    }
                }
            }
            else //끄는 거라면.
            {
                spriteRenderer.sprite = leverOffImage;
                if (isControlOneObject) //컨트롤 할게 한 오브젝트 뿐일경우
                {
                    pulleyPlatformList[0].isCanMove = false;
                    pulleyPlatformList[0].currentRouteIndex += -1 * pulleyRouteList[0];

                }
                else //아니면...
                {
                    for (int i = 0; i < pulleyPlatformList.Count; i++)
                    {

                        pulleyPlatformList[i].isCanMove = false;


                        pulleyPlatformList[i].currentRouteIndex += -1 * pulleyRouteList[i];
                        Debug.Log(i + "번째 도르래 인덱스 : " + pulleyPlatformList[i].currentRouteIndex);

                    }
                }
            }
        }


    }



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(gameObject.transform.position, rangeDistance);
    }


    #region 실험했던것
    //private void FixedUpdate()
    //{
    //    if (isActive)
    //    {
    //        if (isControlOneObject) //컨트롤 할게 한 오브젝트 뿐일경우
    //        {
    //            pulleyPlatformList[0].MovePlatformThisRoute(pulleyPlatformList[0].routeList[0].transform.position);
    //        }
    //        else //아니면...
    //        {
    //            for (int i = 0; i < pulleyPlatformList.Count; i++)
    //            {
    //                pulleyPlatformList[i].MovePlatformThisRoute(pulleyPlatformList[i].routeList[pulleyRouteList[i]].transform.position);
    //                Debug.Log(pulleyPlatformList[i].routeList[pulleyRouteList[i]].transform.position);
    //            }
    //        }
    //    }
    //    else // 액티브 상태가 아니면
    //    {
    //        if (isControlOneObject) //컨트롤 할게 한 오브젝트 뿐일경우
    //        {
    //            pulleyPlatformList[0].MovePlatformThisRoute(pulleyPlatformList[0].routeList[0].transform.position);
    //        }
    //        else //아니면...
    //        {
    //            for (int i = 0; i < pulleyPlatformList.Count; i++)
    //            {
    //                pulleyPlatformList[i].MovePlatformThisRoute(pulleyPlatformList[i].routeList[pulleyRouteList[i]].transform.position);

    //            }
    //        }

    //    }
    //}
    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        if (InputManager.Instance.buttonCatch.wasPressedThisFrame)
    //        {
    //            ToggleLeverActive();
    //            Debug.Log("toggle! : " + isActive);

    //            if (isActive == true) //레버가 온! 되었다면?
    //            {
    //                spriteRenderer.sprite = leverOnImage;
    //                if (isControlOneObject) //컨트롤 할게 한 오브젝트 뿐일경우
    //                {
    //                    if (pulleyPlatformList[0].isCanMove==false) //이미 true라면 아무것도 안함
    //                    {
    //                        pulleyPlatformList[0].isCanMove = true;
    //                    }

    //                    pulleyPlatformList[0].currentRouteIndex += pulleyRouteList[0]; //할당량만큼 더하기.
    //                }
    //                else //아니면...
    //                {
    //                    for (int i = 0; i < pulleyPlatformList.Count; i++)
    //                    {
    //                        if (pulleyPlatformList[i].isCanMove==false) //이미 true라면 아무것도 안함
    //                        {
    //                            pulleyPlatformList[i].isCanMove = true;
    //                        }

    //                        pulleyPlatformList[i].currentRouteIndex += pulleyRouteList[i];

    //                    }
    //                }
    //            }
    //            else //끄는 거라면.
    //            {
    //                spriteRenderer.sprite = leverOffImage;
    //                if (isControlOneObject) //컨트롤 할게 한 오브젝트 뿐일경우
    //                {
    //                    if (pulleyPlatformList[0].isCanMove==true) //마찬가지로 이미 false라면 그냥 놔둔다.
    //                    {
    //                        pulleyPlatformList[0].isCanMove = false;

    //                    }
    //                    pulleyPlatformList[0].currentRouteIndex -= pulleyRouteList[0];
    //                }
    //                else //아니면...
    //                {
    //                    for (int i = 0; i < pulleyPlatformList.Count; i++)
    //                    {
    //                        if (pulleyPlatformList[i].isCanMove==true)
    //                        {
    //                            pulleyPlatformList[i].isCanMove = false;
    //                        }

    //                        pulleyPlatformList[i].currentRouteIndex -= pulleyRouteList[i];

    //                    }
    //                }
    //            }
    //        }

    //    }
    //}

    //private void UpdatePulleyPlatform()
    //{
    //    if (isActive)
    //    {
    //        spriteRenderer.sprite = leverOnImage;
    //        if (isControlOneObject) //컨트롤 할게 한 오브젝트 뿐일경우
    //        {
    //            pulleyPlatformList[0].isCanMove = true;
    //            // pulleyPlatformList[0].MovePlatformThisRoute(pulleyPlatformList[0].routeList[0].transform.position);
    //        }
    //        else //아니면...
    //        {
    //            for (int i = 0; i < pulleyPlatformList.Count; i++)
    //            {
    //                pulleyPlatformList[i].isCanMove = true;
    //                // pulleyPlatformList[i].MovePlatformThisRoute(pulleyPlatformList[i].routeList[pulleyRouteList[i]].transform.position);
    //            }
    //        }
    //    }
    //    else // 액티브 상태가 아니면
    //    {
    //        spriteRenderer.sprite = leverOffImage;
    //        if (isControlOneObject) //컨트롤 할게 한 오브젝트 뿐일경우
    //        {
    //            pulleyPlatformList[0].isCanMove = false;
    //            // pulleyPlatformList[0].MovePlatformThisRoute(pulleyPlatformList[0].routeList[0].transform.position);
    //        }
    //        else //아니면...
    //        {
    //            for (int i = 0; i < pulleyPlatformList.Count; i++)
    //            {
    //                pulleyPlatformList[i].isCanMove = false;
    //                // pulleyPlatformList[i].MovePlatformThisRoute(pulleyPlatformList[i].routeList[pulleyRouteList[i]].transform.position);
    //            }
    //        }

    //    }
    //}
    #endregion

}
