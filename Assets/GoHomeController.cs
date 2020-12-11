using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoHomeController : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(WaitTwoSeconds());
    }

    IEnumerator WaitTwoSeconds()
    {
        yield return new WaitForSeconds(1.5f);
        SceneChanger.Instance.LoadScene("HomeScene");
    }
}
