using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// UI를 관리하는 매니저...
/// 역할별로 나누려다가 그냥 통합했습니다.
/// </summary>
public class UIManager : SingleTon<UIManager>
{
    public Slider MasterVolumeSilder;
    public Slider BgmVolumeSlider;
    public Slider SfxVolimeSlider;
    protected override void Awake()
    {
        base.Awake();
        //Init();
    }
    protected override void Init()
    {
        base.Init();
        CursorInit();
        CanvasObjectDictSetting();
        PiriInit();

    }
    private void Start()
    {
        StartCoroutine(InitCursor());
        StartCoroutine(ProcessPiriMask());
        StartCoroutine(ProcessPiriGaugeImage());

    }

    #region CanvasManager

    public List<GameObject> canvasObjectList;
    private Dictionary<string, GameObject> canvasObjectDict;


    private void CanvasObjectDictSetting()
    {
        Debug.Log("CanvasObjectDictSetting...");
        canvasObjectDict = new Dictionary<string, GameObject>();
        foreach (var item in canvasObjectList)
        {
            canvasObjectDict.Add(item.gameObject.name, item.gameObject);
            Debug.Log("딕셔너리 추가! 키 : " + canvasObjectDict[item.gameObject.name]);

        }
    }

    public void GoQuit()
    {
        Application.Quit();
    }
    public void SetActiveThisCanvasObject(string _name, bool _act)
    {
        canvasObjectDict[_name].SetActive(_act);
    }

    public void SetActiveThisObject(GameObject _gameObject, bool _act)
    {
        _gameObject.SetActive(_act);
    }
    public void ActiveToggleThisCanvasObject(string _name)
    {
        canvasObjectDict[_name].SetActive(!canvasObjectDict[_name].activeInHierarchy);
    }
    public void ActiveToggleThisObject(GameObject _gameObject)
    {
        _gameObject.SetActive(!_gameObject.activeInHierarchy);
    }

    public void SetActiveTrueOnlyThisCanvasObject(string _name)
    {

        GameObject _tempObject = canvasObjectDict[_name];

        foreach (var item in canvasObjectList)
        {
            if (item.gameObject == _tempObject)
            {
                item.gameObject.SetActive(true);
                continue;
            }

            if (item.gameObject.activeInHierarchy == true)
            {
                item.gameObject.SetActive(false);
            }
            else
            {
                continue;
            }
        }
    }

    #endregion

    #region HUDManager

    public bool isCanChangeHUD;

    private bool isCanMaskChange;
    [Header("피리")]
    [Tooltip("피리 게이지 채워지는 부분의 이미지")]
    public Image piriFilledImage;

    [Tooltip("피리 마스크 이미지.")]
    public Image piriMaskImage;

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
        PiriManager.Instance.TEST_PIRICOUNTMINUSONE();
    }

    private void PiriInit()
    {
        isCanMaskChange = false;
        isCanChangeHUD = true;
        oldPiriImagePer = PiriManager.Instance.currentPiriEnergyPer;
        piriUITimer = 0f;

        if (piriFilledSpeed == 0f)
        {
            piriFilledSpeed = 1f;
        }

        currentPiriFilledSpeed = 1f / piriFilledSpeed;

        piriFilledImage.color = new Color32(146, 237, 137, 255);

        piriHighColor = new Color32(146, 237, 137, 255);
        piriLowColor = new Color32(255, 88, 88, 255);

    }

    private IEnumerator ProcessPiriMask()
    {
        while (isCanChangeHUD)
        {
            if (PiriManager.Instance.ctrlInputPer > 0f)
            {
                piriMaskImage.fillAmount = PiriManager.Instance.ctrlInputPer;
                Debug.Log(PiriManager.Instance.ctrlInputPer);
            }
            else if (PiriManager.Instance.ctrlInputPer <= 0f && isCanMaskChange)
            {
                piriMaskImage.fillAmount = 0f;
            }
            else
            {

            }

            yield return YieldInstructionCache.WaitForEndOfFrame;
        }
    }

    private IEnumerator ProcessPiriGaugeImage()
    {
        while (isCanChangeHUD)
        {
            if (PiriManager.Instance.currentPiriEnergyPer != oldPiriImagePer)
            {
                yield return StartCoroutine(LerpOldPiriGaugeImagePer());
            }
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

    }

    private IEnumerator LerpOldPiriGaugeImagePer()
    {
        piriUITimer = 0f;
        realTimer = 0f;

        float tempImagePer = oldPiriImagePer;
        while (Mathf.Abs(PiriManager.Instance.currentPiriEnergyPer - oldPiriImagePer) > 0.005f)
        {
            isCanMaskChange = false;
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
        yield return new WaitForSeconds(1f);
        isCanMaskChange = true;
        PiriManager.Instance.ctrlInputPer = 0f;
    }

    #endregion

    #region CursorManager

    [Header("마우스 커서")]

    [Tooltip("마우스 커서를 변경해도 될까? "),HideInInspector]
    public bool isCanChangeCursor;
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
        while (true)
        {
            if (isCanChangeCursor)
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
                    Cursor.SetCursor(cursorTextures[0], hotspot, CursorMode.Auto);
                }
            }
            else
            {
                Cursor.SetCursor(cursorTextures[2], hotspot, CursorMode.Auto);
            }
           

            yield return YieldInstructionCache.WaitForEndOfFrame;

        }
    }

    #endregion

}
