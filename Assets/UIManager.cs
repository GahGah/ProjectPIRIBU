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

    }
    private void Start()
    {
        StartCoroutine(InitCursor());
    }
    private void Update()
    {

    }




    #region 커서 부분
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
