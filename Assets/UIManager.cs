using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// UI를 관리하는 매니저...
/// 역할별로 나누려다가 그냥 통합했습니다.
/// </summary>
public class UIManager : MonoBehaviour
{

    [Tooltip("Destroy를 하고 싶지 않아서 독자적인 싱글턴 사용")]
    public static UIManager Instance;

    private float gobVal;
    public Slider MasterVolumeSilder;
    public Slider BgmVolumeSlider;
    public Slider SfxVolumeSlider;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        Init();
    }

    public void Init()
    {
        CursorInit();
        PiriInit();
        CanvasObjectDictSetting();
        InitChildButtons();
    }
    private void Start()
    {

        InitSound();

        StartCoroutine(InitCursor());
        StartCoroutine(ProcessPiriMask());
        StartCoroutine(ProcessPiriGaugeImage());

    }


    #region CanvasManager

    public List<GameObject> canvasObjectList;
    public Dictionary<string, GameObject> canvasObjectDict;


    private void CanvasObjectDictSetting()
    {
        canvasObjectDict = new Dictionary<string, GameObject>();
        foreach (var item in canvasObjectList)
        {

            canvasObjectDict.Add(item.gameObject.name, item.gameObject);

        }
    }

    public void PauseToggle()
    {
        SetActiveThisObject(canvasObjectDict["PopUpCanvas"], !canvasObjectDict["PopUpCanvas"].activeInHierarchy);
        if (canvasObjectDict["PopUpCanvas"].activeInHierarchy)
        {
            GoPause();
        }
        else
        {
            GoDePause();
            SetActiveThisObject(canvasObjectDict["QuitCanvas"], false);
            SetActiveThisObject(canvasObjectDict["SettingsCanvas"], false);
        }
    }

    public void InitSound()
    {
        OnChangeMasterSlider();
        OnChangeBGMSlider();
        OnChangeSFXSlider();
    }
    public void OnChangeMasterSlider()
    {
        SoundManager.Instance.audioMixer.SetFloat("masterVolume", Mathf.Log(Mathf.Lerp(0.001f, 1f, MasterVolumeSilder.value)) * 20);
    }

    public void OnChangeBGMSlider()
    {
        SoundManager.Instance.audioMixer.SetFloat("bgmVolume", Mathf.Log(Mathf.Lerp(0.001f, 1f, BgmVolumeSlider.value)) * 20);
    }

    public void OnChangeSFXSlider()
    {
        SoundManager.Instance.audioMixer.SetFloat("sfxVolume", Mathf.Log(Mathf.Lerp(0.001f, 1f, SfxVolumeSlider.value)) * 20);
        SoundManager.Instance.PlaySFX();
    }

    public void OnChangeUISlider()
    {

    }
    public void GoHome()
    {
         SceneChanger.Instance.LoadScene("HomeScene");

    }
    public void GoGameStart()
    {
        SceneChanger.Instance.LoadScene("CutScene_Prologue");
    }
    public void GoQuit()
    {
        Application.Quit();
    }

	public void GoPause()
    {

        Time.timeScale = 0f;

    }

    public void GoDePause()
    {
        Time.timeScale = 1f;
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
               // Debug.Log(PiriManager.Instance.ctrlInputPer);
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

    [Tooltip("마우스 커서를 변경해도 될까? "), HideInInspector]
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
                if (InputManager.Instance.buttonMouseLeft.wasPressedThisFrame)
                {
                    Cursor.SetCursor(cursorTextures[1], hotspot, CursorMode.Auto);
                }
                else if (InputManager.Instance.buttonMouseLeft.wasReleasedThisFrame)
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

    #region MainMenuChild

    public Button[] childButtons;
    [HideInInspector]
    public int[] childClickCount;
    public void InitChildButtons()
    {
        childClickCount = new int[3];
        for (int i = 0; i < childClickCount.Length; i++)
        {
            childClickCount[i] = 0;
        }
        foreach (var item in childButtons)
        {
            item.interactable = true;
            item.image.canvasRenderer.SetAlpha(1f);
        }
    }
    public void DestroyChildToMainMenu(int _index)
    {
        if (childClickCount[_index] ==0)
        {
            childButtons[_index].interactable = false;
            StartCoroutine(DestroyChild(childButtons[_index]));
        }
        else
        {
            childClickCount[_index] -= 1;

        }
    }

    IEnumerator DestroyChild(Button _childButton)
    {
        Image _img = _childButton.image;

        _img.canvasRenderer.SetAlpha(1f);
        float _tempAlpha = 1f;
        float timer = 0f;
        while (_img.canvasRenderer.GetAlpha() != 0f)
        {
            timer += Time.deltaTime * (1f/2f);
            _tempAlpha = Mathf.Lerp(1f, 0f, timer);
            _img.canvasRenderer.SetAlpha(_tempAlpha);

            yield return YieldInstructionCache.WaitForEndOfFrame;

            if (Mathf.Abs(_img.canvasRenderer.GetAlpha() - 0f) <= 0.005f)
            {
                _img.canvasRenderer.SetAlpha(0f);
            }
        }
    }
    #endregion
}
