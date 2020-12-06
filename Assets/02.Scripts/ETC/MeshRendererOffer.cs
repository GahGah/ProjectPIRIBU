using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshRendererOffer : MonoBehaviour
{

    private void Awake()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
}
