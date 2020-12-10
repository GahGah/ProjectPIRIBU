using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode] // 플레이모드가 아니어도 따라다니게....
public class OverlayUIFollower : MonoBehaviour
{
    public Transform targetTransform = null;
    private Vector2 screenPoint = Vector2.zero;

    private void Start()
    {
        screenPoint = Camera.main.WorldToScreenPoint(targetTransform.position);
        if (targetTransform==null)
        {
            Debug.Log(gameObject.name + "의 targetTransform을 설정해줘...");
        }
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
