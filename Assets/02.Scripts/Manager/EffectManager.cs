using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EffectManager : SingleTon<EffectManager>
{
    //접근만 하게 해 주는 매니저... 매니저가 맞나 이쯤되면?
    protected override void Awake()
    {
        base.Awake();

    }
    protected override void Init()
    {
        base.Init();
    }

    [SerializeField] public GameObject VFX_PipeSkill_Green;
    [SerializeField] public GameObject VFX_PipeSkill_Red;

    public void startEffect_VFX_PipeSkill(GameObject startTarget, GameObject endTarget, bool didConsume)
    {
        GameObject VFX;
        if (didConsume == false)
        {
           VFX = Instantiate(VFX_PipeSkill_Green, new Vector3(startTarget.transform.position.x, startTarget.transform.position.y, -5), Quaternion.identity);
        }
        else
        {
            VFX = Instantiate(VFX_PipeSkill_Red, new Vector3(startTarget.transform.position.x, startTarget.transform.position.y, -5), Quaternion.identity);
        }

        VFX.GetComponentInChildren<Script_VFX_PipeSkillEffect>().StartEffect(startTarget, endTarget);
    }
}
