using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShiftKeyImageChange : MonoBehaviour
{
    public Sprite[] catchButtonImages;
    public SpriteRenderer thisKeyImage;
    // Start is called before the first frame update
    void Start()
    {
        if (thisKeyImage == null)
        {
            thisKeyImage = GetComponent<SpriteRenderer>();

        }
        switch (UIManager.Instance.currentCatchButtonName)
        {

            case "leftShift":
                thisKeyImage.sprite = catchButtonImages[0];
                break;

            case "r":

                thisKeyImage.sprite = catchButtonImages[1];
                break;

            case "rightButton":
                thisKeyImage.sprite = catchButtonImages[2];

                break;

            default:

                break;
        }
    }

    private void Update()
    {
        switch (UIManager.Instance.currentCatchButtonName)
        {

            case "leftShift":
                thisKeyImage.sprite = catchButtonImages[0];
                break;

            case "r":

                thisKeyImage.sprite = catchButtonImages[1];
                break;

            case "rightButton":
                thisKeyImage.sprite = catchButtonImages[2];

                break;

            default:

                break;
        }
    }
}
