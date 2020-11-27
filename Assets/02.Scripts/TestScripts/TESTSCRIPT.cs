using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
//키 변경 실험을 위해 만든 스크립트.
public class TESTSCRIPT : MonoBehaviour
{
    #region KeyTest
    //private ButtonControl catchButton = null;
    //public Text KeyUIText = null;

    ////변경 시킬 수 있는 키를 제한시킨다고 했기 때문에,
    ////string으로 선언 후 변경시키는 UI inspector에서 작성
    ////어차피 마우스 우클릭, R, Shift 3개 뿐이니까 그냥 쓰는 게 낫다고 생각...
    ////public string catchButtonName = ""; // R, Shift, leftButton....
    //public void SetCatchButton(string _catchButtonName)
    //{
    //    ButtonControl _control = null;
    //    switch (_catchButtonName)
    //    {
    //        case "rightButton":
    //            _control = (ButtonControl)Mouse.current[_catchButtonName];
    //            break;

    //        case "r":
    //            _control = (ButtonControl)Keyboard.current[_catchButtonName];
    //            break;

    //        case "shift":
    //            _control = (ButtonControl)Keyboard.current[_catchButtonName];
    //            break;

    //        default:
    //            break;
    //    }

    //    //UI update;
    //    KeyUIText.text = _catchButtonName;
    //    catchButton = _control;
    //}
    //private void Awake()
    //{
    //    catchButton = (ButtonControl)Mouse.current["rightButton"];
    //    KeyUIText.text = catchButton.name;
    //}
    //void Update()
    //{
    //    if (catchButton.wasPressedThisFrame)
    //    {
    //        Debug.Log("눌렸다!");
    //    }
    //}
    #endregion

    #region Platform Move Test

    public GameObject startPosObj = null;
    public GameObject endPosObj = null;
    public float moveSpeed;
    public bool isMoving;
    public GameObject playerObj = null;

    public Vector2 oldPosition;

    public bool isPlayer;

    public bool isLoop = true;

    public bool isPlayerRide;

    public float jumpPower;
    public bool isJumping;
    public float jumpVal = 0f;
    public Rigidbody2D playerRigid;
    public Vector2 testPos;

    private void Awake()
    {
        if (moveSpeed <= 0f)
        {
            moveSpeed = 10f;
        }
        isMoving = false;
        isPlayerRide = false;
        oldPosition = Vector2.zero;
        if (jumpPower == 0f)
        {
            jumpPower = 50f;

        }

        isJumping = false;
        if(isPlayer)
        {
            playerRigid = gameObject.GetComponent<Rigidbody2D>();
        }
        else
        {

            playerRigid = playerObj.GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
        if (isPlayer)
        {
            MovePlayer();

        }
        else
        {
            MovePlatform();
        }


    }

    //private void FixedUpdate()
    //{
    //    if (isPlayer)
    //    {
    //        testPos = Vector2.MoveTowards(gameObject.transform.position, gameObject.transform.position + Vector3.up * jumpPower, jumpPower * Time.fixedDeltaTime);
    //    }
        
    //}

    IEnumerator JumpPlayer()
    {
        isJumping = true;
        float timer = 0f;
        while (timer < 0.5f)
        {
            timer += Time.smoothDeltaTime;

          gameObject.transform.position += Vector3.up * (jumpPower);
            yield return new WaitForFixedUpdate();
        }
        isJumping = false;


    }
    public void MovePlayer()
    {


        if (Keyboard.current.aKey.isPressed)
        {

            gameObject.transform.position += Vector3.left * moveSpeed;
        }

        if (Keyboard.current.dKey.isPressed)
        {
            gameObject.transform.position += Vector3.right * moveSpeed;
        }

        if (Keyboard.current.wKey.wasPressedThisFrame && !isJumping)
        {
           // StartCoroutine(JumpPlayer());
            playerRigid.MovePosition(playerRigid.position+Vector2.up*jumpPower);

        }      
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {

            playerRigid.AddForce(Vector2.up * 20f,ForceMode2D.Impulse);

        }

    }
    public void MovePlatform()
    {

        Vector2 _prevPosition = gameObject.transform.position;
        oldPosition = gameObject.transform.position;

        if (Vector2.Distance(gameObject.transform.position, endPosObj.transform.position) < 0.1f)//대충 도착했다 하고
        {
            if (isLoop)
            {
                GameObject _tempObj = endPosObj;
                endPosObj = startPosObj;
                startPosObj = _tempObj;
            }
            else
            {
                return;
            }
        }



        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, endPosObj.transform.position, moveSpeed * Time.smoothDeltaTime);


        if (gameObject.transform.position.y < oldPosition.y)//아래로 갔다면.
        {
            jumpVal = 0f;
        }
        else
        {

             jumpVal = Mathf.Abs(((gameObject.transform.position - endPosObj.transform.position).normalized * moveSpeed).y);
            //jumpVal = Mathf.Abs(gameObject.transform.position.y - endPosObj.transform.position.y);
        }



        if (playerObj.GetComponent<Rigidbody2D>().velocity.y <= 0f && isPlayerRide)
        {
            playerObj.transform.position += (Vector3)gameObject.transform.position - (Vector3)oldPosition;
        }

        if ((Vector2)gameObject.transform.position != oldPosition)
        {
            isMoving = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("TESTTAG"))
        {
            isPlayerRide = true;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.CompareTag("TESTTAG"))
        {
            isPlayerRide = false;

            //  playerRigid.MovePosition(playerRigid.position +Vector2.up * jumpVal);
            //  playerObj.transform.position += Vector3.up * jumpVal;

            playerRigid.velocity += (Vector2.up * jumpVal) * Time.fixedDeltaTime;

        }


    }
    #endregion


}
