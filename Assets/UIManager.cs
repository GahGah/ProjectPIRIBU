using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI를 관리하는 매니저...
/// </summary>
public class UIManager : SingleTon<UIManager>
{


    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    protected override void Init()
    {
        CursorInit();
        PiriInit();

    }
    private void Start()
    {
        StartCoroutine(InitCursor());
        StartCoroutine(ProcessPiriImage());
    }
    private void Update()
    {

        currentPiriFilledSpeed = 1f/piriFilledSpeed;
    }


    #region 피리 HUD 부분
    [Header("피리")]
    [Tooltip("피리 게이지 채워지는 부분의 이미지")]
    public Image piriFilledImage;

    [Tooltip("몇 초 동안 변화할 것인가.")]
    public float piriFilledSpeed;

    [SerializeField]
    private float currentPiriFilledSpeed;

    private float oldPiriImagePer;

    private float piriUITimer;

    private Color32 piriLowColor;
    private Color32 piriHighColor;


    public float realTimer = 0f;
    public void Button_MinusOnePiriEnergy()
    {
        PiriManager.Instance.currentPiriEnergy -= 1;
    }

    private void PiriInit()
    {
        oldPiriImagePer = PiriManager.Instance.currentPiriEnergyPer;
        piriUITimer = 0f;

        if (piriFilledSpeed == 0f)
        {
            piriFilledSpeed = 1f;
        }

        currentPiriFilledSpeed = 1f / piriFilledSpeed;

        piriFilledImage.color = new Color32(146,237,137,255);

        piriHighColor = new Color32(146, 237, 137, 255);
        piriLowColor = new Color32(255, 88, 88, 255);
    }
    private IEnumerator ProcessPiriImage()
    {
        while (true)
        {
            if (PiriManager.Instance.currentPiriEnergyPer != oldPiriImagePer)
            {
                yield return StartCoroutine(LerpOldPiriImagerPer());
            }
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

    }



    private IEnumerator LerpOldPiriImagerPer()
    {
        Debug.Log("Update...");
        piriUITimer = 0f;
        realTimer = 0f;

        float tempImagePer = oldPiriImagePer;
        while (Mathf.Abs(PiriManager.Instance.currentPiriEnergyPer - oldPiriImagePer) > 0.005f)
        {
            piriUITimer += Time.smoothDeltaTime * currentPiriFilledSpeed;
            realTimer += Time.smoothDeltaTime;
            oldPiriImagePer = Mathf.Lerp(tempImagePer, PiriManager.Instance.currentPiriEnergyPer, piriUITimer);
            piriFilledImage.color = Color32.Lerp(piriLowColor, piriHighColor, oldPiriImagePer);

            piriFilledImage.fillAmount = oldPiriImagePer;

            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
        //그냥 딱 맞게 조정.

        oldPiriImagePer = PiriManager.Instance.currentPiriEnergyPer;
        piriFilledImage.color = Color32.Lerp(piriLowColor, piriHighColor, oldPiriImagePer);
        piriFilledImage.fillAmount = oldPiriImagePer;
        Debug.Log("UpdateComplete!");
    }

    #endregion

    #region 커서 부분

    [Header("마우스 커서")]

    [Tooltip("마우스 커서를 변경해도 될까? "), SerializeField]
    private bool isCanChangeCursor;
    Vector2 hotspot;
    public Texture2D[] cursorTextures;

    private void CursorInit()
    {
        hotspot = Vector2.zero;
        isCanChangeCursor = true;

    }
    IEnumerator InitCursor()
    {

        yield return YieldInstructionCache.WaitForEndOfFrame;
        Cursor.SetCursor(cursorTextures[0], hotspot, CursorMode.Auto);
        //Cursor.SetCursor(cursorTexture, hotspot, );
        StartCoroutine(ChangeCursor());
    }

    IEnumerator ChangeCursor()
    {
        while (isCanChangeCursor)
        {
            if (InputManager.instance.buttonMouseLeft.wasPressedThisFrame)
            {
                Cursor.SetCursor(cursorTextures[1], hotspot, CursorMode.Auto);
            }
            else if (InputManager.instance.buttonMouseLeft.wasReleasedThisFrame)
            {
                Cursor.SetCursor(cursorTextures[0], hotspot, CursorMode.Auto);
            }
            else
            {
            }

            yield return YieldInstructionCache.WaitForEndOfFrame;

        }
    }

    #endregion

}
