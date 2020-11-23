using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

public class ShaderManager : SingleTon<GameManager>
{
    public float curTime;
    [SerializeField] private int maxTime;
    [SerializeField] private float timeMultiple;

    [SerializeField] private int time_moringStart;
    [SerializeField] private int time_noonStart;
    [SerializeField] private int time_eveningStart;
    [SerializeField] private int time_nightStart;

    [SerializeField] private Color FogColor_morning;
    [SerializeField] private Color FogColor_noon;
    [SerializeField] private Color FogColor_evening;
    [SerializeField] private Color FogColor_night;

    [SerializeField] private Color LightColor_morning;
    [SerializeField] private Color LightColor_noon;
    [SerializeField] private Color LightColor_evening;
    [SerializeField] private Color LightColor_night;
    //public Color AmbientColor;

    public GameObject mainLight;
    private Light2D mainLight_L2D;

    // Start is called before the first frame update
    void Start()
    {
        mainLight_L2D = mainLight.GetComponent<Light2D>();
        getSpriteList();
        setSpritesFogLevel();
    }

    // Update is called once per frame
    void Update()
    {
        timer();
        changeLightAndFogColor();

        //setSpritesFogLevel();
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

    //이거 최적화 어쩌냐 이거괜찮은건가 젠장
    void changeLightAndFogColor()
    {
        Color fogCol;
        Color LightCol;
        int timeInterval;

        //시간에 따른 색상 구함: Lerp를 사용해서
        if (time_moringStart <= curTime && curTime <= time_noonStart)
        {
            timeInterval = Mathf.Abs(time_moringStart - time_noonStart);
            fogCol = Color.Lerp(FogColor_morning, FogColor_noon, (curTime - time_moringStart) / timeInterval);
            LightCol = Color.Lerp(LightColor_morning, LightColor_noon, (curTime - time_moringStart) / timeInterval);
        }
        else if (curTime <= time_eveningStart)
        {
            timeInterval = Mathf.Abs(time_noonStart - time_eveningStart);
            fogCol = Color.Lerp(FogColor_noon, FogColor_evening, (curTime - time_noonStart) / timeInterval);
            LightCol = Color.Lerp(LightColor_noon, LightColor_evening, (curTime - time_noonStart) / timeInterval);
        }
        else if (curTime <= time_nightStart)
        {
            timeInterval = Mathf.Abs(time_eveningStart - time_nightStart);
            fogCol = Color.Lerp(FogColor_evening, FogColor_night, (curTime - time_eveningStart) / timeInterval);
            LightCol = Color.Lerp(LightColor_evening, LightColor_night, (curTime - time_eveningStart) / timeInterval);
        }
        else
        {
            timeInterval = Mathf.Abs(time_nightStart - maxTime);
            fogCol = Color.Lerp(FogColor_night, FogColor_morning, (curTime - time_nightStart) / timeInterval);
            LightCol = Color.Lerp(LightColor_night, LightColor_morning, (curTime - time_nightStart) / timeInterval);

        }

        // 모든 스프라이트 & 메인 라이트에 적용
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.sharedMaterial.SetColor("_FogColor", fogCol);
        }
        mainLight_L2D.color = LightCol;
    }
}
