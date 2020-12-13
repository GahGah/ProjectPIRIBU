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
    [SerializeField]
    public GameObject VFX_PipeSkill;
}
