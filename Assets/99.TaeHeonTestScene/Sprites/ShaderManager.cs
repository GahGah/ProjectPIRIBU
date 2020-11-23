using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShaderManager : SingleTon<GameManager>
{
    public float curTime;
    public int maxTime;
    public float timeMultiple;

    public Color FogColor_moring;
    public Color FogColor_noon;
    public Color FogColor_evening;
    public Color FogColor_night;
    //public Color AmbientColor;

    // Start is called before the first frame update
    void Start()
    {
        getSpriteList();
        setSpritesFogLevel();
    }

    // Update is called once per frame
    void Update()
    {
        timer();
        changeFogColor();
    }

    //------------------------------
    private SpriteRenderer[] spriteRenderers;
    private List<GameObject> spriteObjects;

    void getSpriteList()
    {
        //모든 스프라이트 오브젝트를 로드함.
        spriteRenderers = GameObject.FindObjectsOfType(typeof(SpriteRenderer)) as SpriteRenderer[];
        spriteObjects = new List<GameObject>();

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            spriteObjects.Add(sr.gameObject);
        }
    }

    void setSpritesFogLevel()
    {
        //스프라이트의 Order in Layer에 따라 각 마테리얼의 FogLevel 값을 바꿔줌.
        foreach (GameObject gameObj in spriteObjects)
        {
            int orderNum = gameObj.GetComponent<SpriteRenderer>().sortingOrder;
            gameObj.GetComponent<SpriteRenderer>().material.SetInt("_FogLevel", orderNum);
        }
    }

    void timer()
    {
        curTime += Time.deltaTime * timeMultiple;

        if(curTime >= maxTime)
        {
            curTime = 0;
        }
    }

    void changeFogColor()
    {
        Color fogCol;
        fogCol = Color.Lerp(FogColor_noon, FogColor_evening, curTime/100);

        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.sharedMaterial.SetColor("_FogColor", fogCol);
        }
    }
}
