using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_VFX_PipeSkillEffect : MonoBehaviour
{
    /* 주의사항
     * ParticleSystem.emission.enabled = true; --> Doesn't compile
     * 
     * var emission = ParticleSystem.emission; -> Stores the module in a local variable
     * emission.enabled = true; -> Applies the new value directly to the Particle System
     */
    [SerializeField]
    private new ParticleSystem particleSystem;

    private IEnumerator cor_ExecuteEffect;
    private ParticleSystem.Particle[] particles;

    [SerializeField]
    private GameObject testTarget;

    private int count;

    private void Awake()
    {
        particleSystem = this.GetComponent<ParticleSystem>();

        particleSystem.Clear();
        particleSystem.Stop();
    }

    private void Start()
    {
        testStart();
    }

    /*--------------------------------------*/
    public void testUpdate()
    {
        count = particleSystem.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            ParticleSystem.Particle particle = particles[i];

            Vector3 v1 = particleSystem.transform.TransformPoint(particle.position);
            Vector3 v2 = testTarget.transform.position + this.transform.position;

            Vector3 tarPosi = (v2 - v1) * (particle.remainingLifetime / particle.startLifetime);
            particle.position = particleSystem.transform.InverseTransformPoint(v2 - tarPosi);
            particles[i] = particle;
        }

        particleSystem.SetParticles(particles, count);
    }

    public void testStart()
    {
        particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
        particleSystem.Play();

        cor_ExecuteEffect = ExecuteEffect(testTarget);
        StartCoroutine(cor_ExecuteEffect);
    }


    public void StartEffect(GameObject startTarget, GameObject endTarget)
    {
        this.transform.position.Set(startTarget.transform.position.x, startTarget.transform.position.y, -9);
        particles = new ParticleSystem.Particle[particleSystem.main.maxParticles];
        particleSystem.Play();

        cor_ExecuteEffect = ExecuteEffect(endTarget);
        StartCoroutine(cor_ExecuteEffect);
    }

    IEnumerator ExecuteEffect(GameObject endTarget)
    {
        //yield return new WaitForSeconds(0.1f);
        Vector3 thisPos = this.transform.position;
        var velocityRadical = particleSystem.velocityOverLifetime.radial;
        float valocityRadicalConstant = velocityRadical.constant;

        bool isAllStop = false;

        while (true)
        {
            yield return null;
            isAllStop = true;

            particles.Initialize();
            int count = particleSystem.GetParticles(particles);
            

            Vector3 velocity = Vector3.zero;
            for (int i = 0, stopCount = 0; i < count; i++)
            {
                ParticleSystem.Particle tempParticle = particles[i];

                Vector3 particlePos = particleSystem.transform.TransformPoint(particles[i].position);
                Vector3 targetPos = testTarget.transform.position + this.transform.position;

                float lifeRate = (tempParticle.remainingLifetime / tempParticle.startLifetime);
                Vector3 targetPosInv = targetPos - particlePos;
                Vector3 posSetValue = targetPos - (targetPosInv * lifeRate);

                Vector3 smoothPos = Vector3.Lerp(particlePos, posSetValue, Mathf.Pow(lifeRate, 3));

                if (lifeRate < 0.5f)
                {
                    particles[i].velocity = new Vector3(0f, 0f, 0f);
                }
                else
                {
                    isAllStop = false;
                }
                
                velocityRadical.constant = 0;

                particlePos = particleSystem.transform.InverseTransformPoint(smoothPos);

                tempParticle.position = particlePos;
                particles[i] = tempParticle;
            }

            if (particleSystem.IsAlive() == false || isAllStop == true)
            {
                break;
            }

            particleSystem.SetParticles(particles);
        }
            

        StopCoroutine(cor_ExecuteEffect);
        EndEffect();
    }



    public void EndEffect()
    {
        Destroy(this.gameObject);
        particleSystem.Stop();
    }

}
