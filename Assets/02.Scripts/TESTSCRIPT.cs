using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
//키 변경 실험을 위해 만든 스크립트.
public class TESTSCRIPT : MonoBehaviour
{
    private ButtonControl catchButton = null;
    public Text KeyUIText = null;

    //변경 시킬 수 있는 키를 제한시킨다고 했기 때문에,
    //string으로 선언 후 변경시키는 UI inspector에서 작성
    //어차피 마우스 우클릭, R, Shift 3개 뿐이니까 그냥 쓰는 게 낫다고 생각...
    //public string catchButtonName = ""; // R, Shift, leftButton....
    public void SetCatchButton(string _catchButtonName)
    {
        ButtonControl _control = null;
        switch (_catchButtonName)
        {
            case "rightButton":
                _control = (ButtonControl)Mouse.current[_catchButtonName];
                break;

            case "r":
                _control = (ButtonControl)Keyboard.current[_catchButtonName];
                break;

            case "shift":
                _control = (ButtonControl)Keyboard.current[_catchButtonName];
                break;

            default:
                break;
        }

        //UI update;
        KeyUIText.text = _catchButtonName;
        catchButton = _control;
    }
    private void Awake()
    {
        catchButton = (ButtonControl)Mouse.current["rightButton"];
        KeyUIText.text = catchButton.name;
    }
    void Update()
    {
        if (catchButton.wasPressedThisFrame)
        {
            Debug.Log("눌렸다!");
        }
    }
}
