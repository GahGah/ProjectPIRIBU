using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulleyBalphan : PlatformController
{
    public SpriteRenderer spriteRenderer;

    [Tooltip("컨트롤할 도르래 플랫폼의 리스트")]
    public List<PulleyPlatform> pulleyPlatformList;

    [Tooltip("이동할 경로의 인덱스(PulleyPlatform의 routeList Index. \n일단 컨트롤오브젝트리스트당 하나씩만 넣는 것으로 따졌음~)")]
    public int[] pulleyRouteList;

    [Header("[임시] 발판의 온/오프 이미지. ")]
    public Sprite balphanOnImage;
    public Sprite balphanOffImage;

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

    private void Update()
    {
        
        if (Vector2.Distance(GameManager.Instance.hero.transform.position, gameObject.transform.position) < rangeDistance)
        {

            if (!isCanActive)
            {
                return;
            }

            SetIsActive(true);
            //Debug.Log("toggle! : " + isActive);


            spriteRenderer.sprite = balphanOnImage;
            if (isControlOneObject) //컨트롤 할게 한 오브젝트 뿐일경우
            {
                if (pulleyPlatformList[0].IsCanControl() == true)
                {
                    if (pulleyPlatformList[0].isCanMove == true)
                    {

                    }
                    else
                    {
                        pulleyPlatformList[0].isCanMove = true;
                        pulleyPlatformList[0].isStopped = false;
                        pulleyPlatformList[0].currentRouteIndex += pulleyRouteList[0]; //할당량만큼 더하기.
                    }

                   
                }

            }
            else //아니면...
            {
                for (int i = 0; i < pulleyPlatformList.Count; i++)
                {
                    if (pulleyPlatformList[i].IsCanControl() == true) //조종할 수 있는 상태라면
                    {

                        if (pulleyPlatformList[i].isCanMove == true)
                        {

                        }
                        else
                        {
                            pulleyPlatformList[i].isCanMove = true;
                            pulleyPlatformList[i].isStopped = false;

                            pulleyPlatformList[i].currentRouteIndex += pulleyRouteList[i];
                        }


                    }


                }
            }
        }
        else //끄는 거라면. (발판에서 멀어지면 false가 되어야함.
        {
            SetIsActive(false);
            //Debug.Log("toggle! : " + isActive);


            spriteRenderer.sprite = balphanOffImage;
            if (isControlOneObject) //컨트롤 할게 한 오브젝트 뿐일경우
            {
                if (pulleyPlatformList[0].IsCanControl() == true)
                {
                    pulleyPlatformList[0].isCanMove = false;
                    pulleyPlatformList[0].isStopped = true;
                    //pulleyPlatformList[0].currentRouteIndex += -1 * pulleyRouteList[0];
                }
            }
            else //아니면...
            {
                for (int i = 0; i < pulleyPlatformList.Count; i++)
                {
                    if (pulleyPlatformList[i].IsCanControl() == true) //조종할 수 있는 상태라면
                    {
                        pulleyPlatformList[i].isCanMove = false;
                        pulleyPlatformList[i].isStopped = true;

                        // pulleyPlatformList[i].currentRouteIndex += -1 * pulleyRouteList[i];
                        
                    }
                }
            }
        }



    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(gameObject.transform.position, rangeDistance);
    }

}
