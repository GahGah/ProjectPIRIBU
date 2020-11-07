using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class TestClassNameCreate : MonoBehaviour
{

    private void Awake()
    {
        TestClassCreate();
        
    }
    string testName = "PlayerState";

    public void TestClassCreate()
    {
        Assembly assem = Assembly.GetExecutingAssembly();
        object obj = assem.CreateInstance(testName);

        if (obj is TestClassOne)
        {
            Debug.Log("testOK");
        }
    }

}
public class TestClassOne : MonoBehaviour
{
    int a = 10;
}
