using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayUIFollower : MonoBehaviour
{

    public bool isPiriUI;
    public Transform targetTransform = null;
    private Vector2 screenPoint = Vector2.zero;

    private void OnEnable()
    {
        if (isPiriUI == true)
        {
            targetTransform = GameManager.Instance.piriUIPosition;
        }
        if (targetTransform == null)
        {
            Debug.Log(gameObject.name + "의 targetTransform을 설정해줘...");
        }

        screenPoint = Camera.main.WorldToScreenPoint(targetTransform.position);

    }
    private void Start()
    {
        //if (isPiriUI==true)
        //{
        //    targetTransform = GameManager.Instance.piriUIPosition;
        //}
        //screenPoint = Camera.main.WorldToScreenPoint(targetTransform.position);
        //if (targetTransform==null)
        //{
        //    Debug.Log(gameObject.name + "의 targetTransform을 설정해줘...");
        //}
    }
    //void Update()
    //{

    //}

    private void FixedUpdate()
    {
        targetFollow();
    }

    private void targetFollow()
    {
        if (gameObject.activeInHierarchy)
        {
            screenPoint = Camera.main.WorldToScreenPoint(targetTransform.position);
            gameObject.transform.position = screenPoint;
        }
        else
        {

        }
    }
}
