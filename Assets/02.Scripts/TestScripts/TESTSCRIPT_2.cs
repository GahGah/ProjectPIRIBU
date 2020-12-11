using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class TESTSCRIPT_2 : MonoBehaviour, IPointerEnterHandler
{

    public Button testButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter.");
    }
    public void testButtonTestFunction()
    {
    }
}
