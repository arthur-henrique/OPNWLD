using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlobalVolumeScript : MonoBehaviour, IObserver, iDamageObserver, IDeathObserver
{

    private Volume volumeVar;
    //private Bloom bloomVar;
    private Vignette vignetteVar;
    private ChromaticAberration chromaticVar;

    public HealthControl agentHealth;

    void Start()
    {
        volumeVar = GetComponent<Volume>();
        //volumeVar.profile.TryGet(out bloomVar);
        volumeVar.profile.TryGet(out vignetteVar);
        //vignetteVar.intensity = new ClampedFloatParameter(0.5f,0f, 1f,true);
        volumeVar.profile.TryGet(out chromaticVar);
        StartCoroutine(FindPlayer());
    }

    void AddObserver()
    {
        agentHealth.AddDamageObserver(this);
        agentHealth.AddDeathObserver(this);
    }
    public void RemoveObserver()
    {
        agentHealth.RemoveDamageObserver(this);
        agentHealth.RemoveDeathObserver(this);
    }


    public void OnNotifyDeath()
    {

    }

    public void OnNotifyDamage(float damage)
    {
        print("levou dano");
        vignetteVar.color.value = (Color.red);
        StartCoroutine(VignetteValue());
    }

    public void OnNotifyHeal(float heal)
    {
        vignetteVar.color.value = (Color.green);
        StartCoroutine(VignetteValue());
    }

    IEnumerator FindPlayer()
    {
        yield return new WaitForSeconds(0.15f);
        agentHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthControl>();
        AddObserver();

    }

    IEnumerator VignetteValue()
    {
        print("vignette value mudado");
        for (float i = 0; i < 0.456f; i+= 0.0455f)
        {
            vignetteVar.intensity.value = i;
            yield return new WaitForSecondsRealtime(0.034f);
        }
        yield return new WaitForSecondsRealtime(0.025f);
        for (float i = 0.454f; i > -1; i -= 0.0455f)
        {
            vignetteVar.intensity.value = i;
            yield return new WaitForSecondsRealtime(0.034f);
        }

        //vignetteVar.intensity.value = 0.455f;
    }

    //PARA O DRAGAO
    IEnumerator ChromaticValue()
    {
        //Condi��o para iniciar: StartCoroutine(ChromaticValue());
        for (float i = 0; i < 0.539f; i += 0.0538f)
        {
            chromaticVar.intensity.value = i;
            yield return new WaitForSecondsRealtime(0.034f);
        }
        //Colocar aqui a condi��o de quando que o efeito vai acabar:
        //if(dragao parar de rugir) {
        for (float i = 0.537f; i > -1; i -= 0.0538f)
        {
            chromaticVar.intensity.value = i;
            yield return new WaitForSecondsRealtime(0.034f);
        }
    }
}
