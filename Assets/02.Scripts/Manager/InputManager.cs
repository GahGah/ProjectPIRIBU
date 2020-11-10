using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//모든 입력 관리
public class InputManager : SingleTon<InputManager>
{
    private Input input = null;

    public bool isJumpKeyPressed = false;
    public Vector2 moveVal = new Vector2(0, 0);
    public Vector2 jumpVal = new Vector2(0, 0);

    public void Init()
    {
        if (input == null)
        {
            input = new Input();
        }
       
    }

    public void OnMove(InputAction.CallbackContext _context)
    {

        moveVal = _context.ReadValue<Vector2>();

        if (Keyboard.current.upArrowKey.wasPressedThisFrame) //KeyDown
        {
            isJumpKeyPressed = true;
        }
        else
        {
            isJumpKeyPressed = false;
        }
    }

}   
