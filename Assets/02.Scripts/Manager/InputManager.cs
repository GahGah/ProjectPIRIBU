using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

//모든 입력 관리
public class InputManager : SingleTon<InputManager>
{
    public ButtonControl buttonW = Keyboard.current.wKey;
    public ButtonControl buttonA = Keyboard.current.aKey;
    public ButtonControl buttonS = Keyboard.current.sKey;
    public ButtonControl buttonD = Keyboard.current.dKey;
    public ButtonControl buttonSpace = Keyboard.current.spaceKey;
    public ButtonControl buttonMouseLeft = Mouse.current.leftButton;
    public ButtonControl buttonCtrl = Keyboard.current.ctrlKey;
    public ButtonControl buttonESC = Keyboard.current.escapeKey;
    public ButtonControl buttonCatch = Keyboard.current.leftShiftKey;

    //protected override void Init()
    //{
    //    base.Init();
    //}
    /// <summary>
    /// 해당 버튼의 세팅을 변경합니다.
    /// </summary>
    /// <param name="_button">변경하고 싶은 현재 버튼</param>
    /// <param name="_changeButton">변경할 버튼</param>
    public void ChangeButton(ButtonControl _currentButton, ButtonControl _changeButton)
    {
        _currentButton = _changeButton;
    }
    public void ChangeCatchButton(string _catchButtonName)
    {
        switch (_catchButtonName)
        {
            case "rightButton":
                ChangeButton(buttonCatch, Mouse.current.rightButton);
                break;

            case "r":
                ChangeButton(buttonCatch, Keyboard.current.rKey);
                break;

            case "leftShift":
                ChangeButton(buttonCatch, Keyboard.current.leftShiftKey);
                break;

            default:
                ChangeButton(buttonCatch, Mouse.current.leftButton);
                break;
        }
    }



    //private Input input = null;

    //public bool isJumpKeyPressed = false;
    //public Vector2 moveVal = new Vector2(0, 0);
    //public Vector2 jumpVal = new Vector2(0, 0);

    //public void Init()
    //{
    //    if (input == null)
    //    {
    //        input = new Input();
    //    }
       
    //}

    //public void OnMove(InputAction.CallbackContext _context)
    //{

    //    moveVal = _context.ReadValue<Vector2>();

    //    if (Keyboard.current.upArrowKey.wasPressedThisFrame) //KeyDown
    //    {
    //        isJumpKeyPressed = true;
    //    }
    //    else
    //    {
    //        isJumpKeyPressed = false;
    //    }
    //}

}   
