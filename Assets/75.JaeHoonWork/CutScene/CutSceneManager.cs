using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    public GameObject pagePrefab;
    public List<Sprite> sprites;
    public string nextSceneName;
    private List<PageCurl> pages;
    PageCurl page;
    bool disableNext;
    void Start()
    {
        pages = new List<PageCurl>();

        //첫번째 페이지가 가장 마지막에 Instantiate되어야 가장 위에 보이게 되므로 Reverse
        sprites.Reverse();
        foreach (Sprite sprite in sprites)
        {
            page = Instantiate(pagePrefab, transform).GetComponentInChildren<PageCurl>();
            pages.Add(page);
            page.SetSprite(sprite);
        }

        //첫 페이지가 마지막 item이므로 다시 Reverse시킴
        pages.Reverse();
        page = pages[0];
        disableNext = false;
    }

    void GoNextScene()
    {
        if (SceneChanger.Instance != null && !SceneChanger.Instance.isFading)
        {
            SceneChanger.Instance.LoadScene(nextSceneName);
            disableNext = true;
        }
    }
    void Update()
    {
        if (!disableNext)
        {

            //ESC 스킵
            if (InputManager.Instance.buttonPause.wasPressedThisFrame)
            {
                GoNextScene();
            }

            if (InputManager.Instance.buttonMouseLeft.wasPressedThisFrame)
            {

                //넘긴 페이지 삭제
                if (!pages[0])
                {
                    pages.RemoveAt(0);
                    page = pages[0];
                }

                //다음 페이지
                if (pages.Count > 1)
                {
                   
                    page.NextPage();
                    SoundManager.Instance.PlaySFXOneShot(SoundManager.Instance.audioClips[2]);
                }

                //마지막 페이지면 다음씬으로
                else
                    GoNextScene();
            }

        }
    }
}
