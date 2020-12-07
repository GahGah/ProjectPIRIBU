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

    [SerializeField] private int time_morningStart;
    [SerializeField] private int time_noonStart;
    [SerializeField] private int time_eveningStart;
    [SerializeField] private int time_nightStart;
    [SerializeField] private int time_nightEnd;

    [SerializeField] private Color FogColor_morning;
    [SerializeField] private Color FogColor_noon;
    [SerializeField] private Color FogColor_evening;
    [SerializeField] private Color FogColor_night;

    [SerializeField] private Color LightColor_morning;
    [SerializeField] private Color LightColor_noon;
    [SerializeField] private Color LightColor_evening;
    [SerializeField] private Color LightColor_night;

    [SerializeField] private Gradient skyGradient_morning = new Gradient();
    [SerializeField] private Gradient skyGradient_noon = new Gradient();
    [SerializeField] private Gradient skyGradient_evening = new Gradient();
    [SerializeField] private Gradient skyGradient_night = new Gradient();

    [SerializeField] private GameObject skySprite;
    [SerializeField] private GameObject skyStar;
    private Material skyStar_Material;
    [SerializeField] public GameObject mainLight;
    private Light2D mainLight_L2D;


    // Start is called before the first frame update
    void Start()
    {
        mainLight_L2D = mainLight.GetComponent<Light2D>();
        skyStar_Material = skyStar.GetComponent<MeshRenderer>().material;
        getSpriteList();
        setSpritesFogLevel();
    }

    // Update is called once per frame
    void Update()
    {
        timer();
        changeLightAndFogColor();
        changeSunAndMoon();

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

        if (curTime >= maxTime)
        {
            curTime = 0;
        }
    }

    void changeLightAndFogColor()
    {
        Color fogCol;
        Color lightCol;

        Color gradientBegin;
        Color gradientEnd;
        int timeInterval;

        //시간에 따른 색상 구함: Lerp를 사용해서
        if (time_morningStart <= curTime && curTime <= time_noonStart)
        {
            timeInterval = Mathf.Abs(time_morningStart - time_noonStart);
            fogCol = Color.Lerp(FogColor_morning, FogColor_noon, (curTime - time_morningStart) / timeInterval);
            lightCol = Color.Lerp(LightColor_morning, LightColor_noon, (curTime - time_morningStart) / timeInterval);

            gradientBegin = Color.Lerp(skyGradient_morning.colorKeys[0].color, skyGradient_noon.colorKeys[0].color, (curTime - time_morningStart) / timeInterval);
            gradientEnd = Color.Lerp(skyGradient_morning.colorKeys[1].color, skyGradient_noon.colorKeys[1].color, (curTime - time_morningStart) / timeInterval);
        }
        else if (curTime <= time_eveningStart)
        {
            timeInterval = Mathf.Abs(time_noonStart - time_eveningStart);
            fogCol = Color.Lerp(FogColor_noon, FogColor_evening, (curTime - time_noonStart) / timeInterval);
            lightCol = Color.Lerp(LightColor_noon, LightColor_evening, (curTime - time_noonStart) / timeInterval);

            gradientBegin = Color.Lerp(skyGradient_noon.colorKeys[0].color, skyGradient_evening.colorKeys[0].color, (curTime - time_noonStart) / timeInterval);
            gradientEnd = Color.Lerp(skyGradient_noon.colorKeys[1].color, skyGradient_evening.colorKeys[1].color, (curTime - time_noonStart) / timeInterval);
        }
        else if (curTime <= time_nightStart)
        {
            timeInterval = Mathf.Abs(time_eveningStart - time_nightStart);
            fogCol = Color.Lerp(FogColor_evening, FogColor_night, (curTime - time_eveningStart) / timeInterval);
            lightCol = Color.Lerp(LightColor_evening, LightColor_night, (curTime - time_eveningStart) / timeInterval);

            gradientBegin = Color.Lerp(skyGradient_evening.colorKeys[0].color, skyGradient_night.colorKeys[0].color, (curTime - time_eveningStart) / timeInterval);
            gradientEnd = Color.Lerp(skyGradient_evening.colorKeys[1].color, skyGradient_night.colorKeys[1].color, (curTime - time_eveningStart) / timeInterval);
        }
        else if (curTime <= time_nightEnd)
        {
            fogCol = FogColor_night;
            lightCol = LightColor_night;

            gradientBegin = skyGradient_night.colorKeys[0].color;
            gradientEnd = skyGradient_night.colorKeys[1].color;
        }
        else
        {
            timeInterval = Mathf.Abs(time_nightEnd - maxTime);
            fogCol = Color.Lerp(FogColor_night, FogColor_morning, (curTime - time_nightEnd) / timeInterval);
            lightCol = Color.Lerp(LightColor_night, LightColor_morning, (curTime - time_nightEnd) / timeInterval);

            gradientBegin = Color.Lerp(skyGradient_night.colorKeys[0].color, skyGradient_morning.colorKeys[0].color, (curTime - time_nightEnd) / timeInterval);
            gradientEnd = Color.Lerp(skyGradient_night.colorKeys[1].color, skyGradient_morning.colorKeys[1].color, (curTime - time_nightEnd) / timeInterval);
        }

        // 모든 스프라이트 & 메인 라이트에 적용
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.sharedMaterial.SetColor("_FogColor", fogCol);
        }
        mainLight_L2D.color = lightCol;

        skySprite.GetComponent<MeshRenderer>().material.SetColor("_BeginColor", gradientBegin);
        skySprite.GetComponent<MeshRenderer>().material.SetColor("_EndColor", gradientEnd);
    }

    void changeSunAndMoon()
    {
        int timeInterval;

        if (time_morningStart <= curTime && curTime <= time_noonStart)
        {
            timeInterval = Mathf.Abs(time_morningStart - time_noonStart);
            skyStar_Material.SetColor("_BaseColor", Color.Lerp(Color.gray, Color.black, (curTime - time_morningStart) / timeInterval));
        }
        else if (time_noonStart <= curTime && curTime <= time_eveningStart)
        {
            timeInterval = Mathf.Abs(time_noonStart - time_eveningStart);
            skyStar_Material.SetColor("_BaseColor", Color.Lerp(Color.black, Color.gray, (curTime - time_noonStart) / timeInterval));
        }
        else if (time_eveningStart <= curTime && curTime <= time_nightStart)
        {
            timeInterval = Mathf.Abs(time_eveningStart - time_nightStart);
            skyStar_Material.SetColor("_BaseColor", Color.Lerp(Color.gray, Color.white, (curTime - time_eveningStart) / timeInterval));
        }
        else if (time_nightEnd < curTime && curTime <= maxTime)
        {
            timeInterval = Mathf.Abs(time_nightEnd - maxTime);
            skyStar_Material.SetColor("_BaseColor", Color.Lerp(Color.white, Color.gray, (curTime - time_nightEnd) / timeInterval));
        }
    }
}