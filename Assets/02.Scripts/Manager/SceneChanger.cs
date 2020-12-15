using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [Tooltip("Destroy를 하고 싶지 않아서 독자적인 싱글턴 사용")]
    public static SceneChanger Instance;

    public Image loadingBarImage;
    public CanvasGroup canvasGroup;
    public Image whitePanel;

    private float fadeTimer;
    private float currentFadeSpeed;
    private float currentLoadingFadeSpeed;

    private float loadingTimer;

    public string loadSceneName;

    /// <summary>
    /// 씬 로딩 중인가?
    /// </summary>
    public bool isLoading;

    // 페이드인 끝나야만 컷씬 스킵 가능하게 할게요. 바로 누르면 씬 로딩이 제대로 안되어서;
    /// <summary>
    /// 페이딩중인가?
    /// </summary>
    public bool isFading;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        Init();
    }
    private void Init()
    {

        currentFadeSpeed = 1f / 2f;
        currentLoadingFadeSpeed = 1f / 2f;
        loadingBarImage.fillAmount = 0f;
    }


    public void LoadScene(string _sceneName)
    {
        isLoading = true;
        isFading = true;
        //gameObject.SetActive(true);

        SceneManager.sceneLoaded += LoadSceneEnd;
        loadSceneName = _sceneName;

        StartCoroutine(LoadingScene(_sceneName, Color.black));
    }
    private void LoadSceneEnd(Scene scene, LoadSceneMode loadSceneMode)
    {

        if (scene.name == loadSceneName)
        {
            StartCoroutine(FadeAlphaCanvasGroup(1f, 0f));

            SceneManager.sceneLoaded -= LoadSceneEnd;
            if (loadSceneName == "HomeScene")
            {
                SoundManager.Instance.audioSources[0].Stop();
                SoundManager.Instance.PlayBGM(0);
                UIManager.Instance.SetActiveTrueOnlyThisCanvasObject("MainMenuCanvas");
                UIManager.Instance.InitChildButtons();
                UIManager.Instance.GoDePause();

            }
            else if (loadSceneName == "InGameScene")
            {

                PiriManager.Instance.PiriInit();
                UIManager.Instance.SetActiveTrueOnlyThisCanvasObject("HUDCanvas");

            }
            else if (loadSceneName == "CutScene_Prologue")
            {
                UIManager.Instance.SetActiveThisCanvasObject("MainMenuCanvas", false);
            }
            isLoading = false;
        }

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_sceneName">로드하고 싶은 씬의 이름~</param>
    /// <param name="_fadeColor">어떤 색의 배경으로 페이드 인/페이드 아웃 할 것인가?</param>
    /// <returns></returns>
    private IEnumerator LoadingScene(string _sceneName, Color _fadeColor)
    {
        yield return YieldInstructionCache.WaitForEndOfFrame;
        whitePanel.gameObject.SetActive(true);
        loadingBarImage.gameObject.SetActive(true);
        whitePanel.color = _fadeColor;
        loadingBarImage.fillAmount = 0f;

        yield return StartCoroutine(FadeAlphaCanvasGroup(0f, 1f));
        yield return new WaitForSecondsRealtime(0.5f);
        AsyncOperation loadOp = SceneManager.LoadSceneAsync(_sceneName);
        loadOp.allowSceneActivation = false;

        loadingTimer = 0f;

        while (loadOp.isDone == false)
        {
            yield return YieldInstructionCache.WaitForEndOfFrame;
            loadingTimer += Time.unscaledDeltaTime;

            if (loadOp.progress < 0.9f)
            {
                loadingBarImage.fillAmount = Mathf.Lerp(loadingBarImage.fillAmount, loadOp.progress, loadingTimer);
            }
            else
            {
                loadingBarImage.fillAmount = Mathf.Lerp(loadingBarImage.fillAmount, 1f, loadingTimer);
                if (loadingBarImage.fillAmount == 1f)
                {
                    yield return new WaitForSecondsRealtime(1f);
                    loadOp.allowSceneActivation = true;
                    yield break;
                }
            }

        }

    }

    private IEnumerator FadeAlphaCanvasGroup(float _startAlphaValue, float _endAlphaValue)
    {
        canvasGroup.alpha = _startAlphaValue;
        float _tempAlpha = _startAlphaValue;
        fadeTimer = 0f;
        while (canvasGroup.alpha != _endAlphaValue)
        {
            fadeTimer += Time.unscaledDeltaTime * currentFadeSpeed;
            _tempAlpha = Mathf.Lerp(_startAlphaValue, _endAlphaValue, fadeTimer);
            canvasGroup.alpha = _tempAlpha;

            yield return YieldInstructionCache.WaitForEndOfFrame;

            if (Mathf.Abs(canvasGroup.alpha - _endAlphaValue) <= 0.005f)
            {
                canvasGroup.alpha = _endAlphaValue;
            }
        }


        if (_endAlphaValue == 0f)
        {

            whitePanel.gameObject.SetActive(false);
            loadingBarImage.gameObject.SetActive(false);

            //페이드인 끝나야만 컷씬 스킵 가능하게 할게요. 바로 누르면 씬 로딩이 제대로 안되어서;
            isFading = false;
        }
    }
    private IEnumerator FadeAlpha(float _startAlphaValue, float _endAlphaValue)
    {
        canvasGroup.alpha = 1f;
        whitePanel.canvasRenderer.SetAlpha(_startAlphaValue);
        float _tempAlpha = _startAlphaValue;
        fadeTimer = 0f;
        while (whitePanel.canvasRenderer.GetAlpha() != _endAlphaValue)
        {
            Debug.Log("Loading...");
            fadeTimer += Time.unscaledDeltaTime * currentFadeSpeed;
            _tempAlpha = Mathf.Lerp(_startAlphaValue, _endAlphaValue, fadeTimer);
            whitePanel.canvasRenderer.SetAlpha(_tempAlpha);

            yield return YieldInstructionCache.WaitForEndOfFrame;

            if (Mathf.Abs(whitePanel.canvasRenderer.GetAlpha() - _endAlphaValue) <= 0.005f)
            {
                whitePanel.canvasRenderer.SetAlpha(_endAlphaValue);
            }
        }


        if (_endAlphaValue == 0f)
        {
            whitePanel.gameObject.SetActive(false);
            loadingBarImage.gameObject.SetActive(false);
        }
    }

    private IEnumerator FadeAlphaLoadingBar(float _startAlphaValue, float _endAlphaValue)
    {

        if (_startAlphaValue == 1f)
        {
            loadingBarImage.fillAmount = 1f;
        }
        loadingBarImage.canvasRenderer.SetAlpha(_startAlphaValue);
        float _tempAlpha = _startAlphaValue;
        fadeTimer = 0f;
        while (loadingBarImage.canvasRenderer.GetAlpha() != _endAlphaValue)
        {
            fadeTimer += Time.deltaTime * currentLoadingFadeSpeed;
            _tempAlpha = Mathf.Lerp(_startAlphaValue, _endAlphaValue, fadeTimer);
            loadingBarImage.canvasRenderer.SetAlpha(_tempAlpha);

            yield return YieldInstructionCache.WaitForEndOfFrame;

            if (Mathf.Abs(loadingBarImage.canvasRenderer.GetAlpha() - _endAlphaValue) <= 0.005f)
            {
                loadingBarImage.canvasRenderer.SetAlpha(_endAlphaValue);
            }
        }


        if (_endAlphaValue == 0f)
        {
            loadingBarImage.gameObject.SetActive(false);
        }
    }

}
