using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어가 닿으면 알파값이 0.5가 됩니다.
/// </summary>
public class OutsideAlphaController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private IEnumerator nowCoroutine;

    [SerializeField]
    private float alphaVal;
    private float changeSpeed;
    private float limitDis;
    private float changeTimer;
    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        changeSpeed = 1f;
        limitDis = 0.01f;
        alphaVal = 1f;
        changeTimer = 0f;
    }
    private void Start()
    {
        if (GameManager.Instance.isDebugMode!=true)
        {
            alphaVal = 0f;
        }
        nowCoroutine = null;
        Debug.Log(spriteRenderer.color);
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alphaVal);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (nowCoroutine != null)
            {
                StopCoroutine(nowCoroutine);
            }

            nowCoroutine = AlphaGoingVal(0.5f);
            StartCoroutine(nowCoroutine);
            Debug.Log("Exit");
            // spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);

        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Enter");
            if (nowCoroutine != null)
            {
                StopCoroutine(nowCoroutine);
            }

            nowCoroutine = AlphaGoingOne();
            StartCoroutine(nowCoroutine);
            //spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);

        }


    }

    private IEnumerator AlphaGoingOne()
    {
        changeTimer = 0f;
        while (
            Mathf.Abs(alphaVal - 1f) > limitDis
            )
        {
            changeTimer += Time.smoothDeltaTime*changeSpeed;
            alphaVal = Mathf.Lerp(alphaVal, 1f, changeTimer);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b,alphaVal);
            yield return YieldInstructionCache.WaitForEndOfFrame;

        }
    }

    private IEnumerator AlphaGoingVal(float _val)
    {
        changeTimer = 0f;
        while (
            Mathf.Abs(alphaVal - _val) > limitDis
            )
        {
            changeTimer += Time.smoothDeltaTime*changeSpeed;
            alphaVal = Mathf.Lerp(alphaVal, _val, changeTimer);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alphaVal);
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
    }

}
