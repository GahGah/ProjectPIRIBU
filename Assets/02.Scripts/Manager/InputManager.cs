using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

//모든 입력 관리
public class InputManager : SingleTon<InputManager>
{
    public ButtonControl buttonUp;// = Keyboard.current.wKey;
    public ButtonControl buttonLeft; //= Keyboard.current.aKey;
    public ButtonControl buttonDown;// = Keyboard.current.sKey;
    public ButtonControl buttonRight; //= Keyboard.current.dKey;
    public ButtonControl buttonJump;// = Keyboard.current.spaceKey;
    public ButtonControl buttonMouseLeft;// = Mouse.current.leftButton;
    public ButtonControl buttonCtrl;// = Keyboard.current.ctrlKey;
    public ButtonControl buttonPause;// = Keyboard.current.escapeKey;
    public ButtonControl buttonCatch; //= Keyboard.current.leftShiftKey;
    public Vector2Control buttonScroll;// = Mouse.current.scroll;

    public ButtonControl buttonChildFollow;

    private Vector2 mouseCurrentPosition = Vector2.zero;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Init()
    {
        base.Init();

        buttonUp = Keyboard.current.wKey;
        buttonLeft = Keyboard.current.aKey;
        buttonDown = Keyboard.current.sKey;
        buttonRight = Keyboard.current.dKey;
        buttonJump = Keyboard.current.spaceKey;
        buttonMouseLeft = Mouse.current.leftButton;
        buttonCtrl = Keyboard.current.ctrlKey;
        buttonPause = Keyboard.current.escapeKey;
        buttonCatch = Keyboard.current.leftShiftKey;
        buttonScroll = Mouse.current.scroll;
        buttonChildFollow = Keyboard.current.qKey;

    }
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

    public Vector2 GetMouseCurrentPosition()
    {
        mouseCurrentPosition = Mouse.current.position.ReadValue();
        return mouseCurrentPosition;
    }

    public void TESTSCREENPOINTTORAY()
    {
        Ray ray = Camera.main.ScreenPointToRay(InputManager.instance.mouseCurrentPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10000f))
        {

            Vector3 velo = Vector3.zero;
            Vector3 temp = new Vector3(hit.point.x, 20f, hit.point.z);
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
