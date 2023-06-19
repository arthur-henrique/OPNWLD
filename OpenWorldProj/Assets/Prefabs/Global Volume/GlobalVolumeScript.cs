using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlobalVolumeScript : MonoBehaviour, IObserver, IHealingObserver, IDeathObserver
{

    private Volume volumeVar;
    //private Bloom bloomVar;
    private Vignette vignetteVar;
    private ChromaticAberration chromaticVar;

    public HealthControl agentHealth;
    public PlayerManager playerM;

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
        playerM.AddHealObserver(this);
    }
    public void RemoveObserver()
    {
        agentHealth.RemoveDamageObserver(this);
        agentHealth.RemoveDeathObserver(this);
        playerM.RemoveHealObserver(this);

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

    public void OnNotifyHeal()
    {
        vignetteVar.color.value = (Color.green);
        StartCoroutine(VignetteValue());
    }

    IEnumerator FindPlayer()
    {
        yield return new WaitForSeconds(0.15f);
        agentHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthControl>();
        playerM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PlayerManager>();

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

    public void Roar()
    {
        StartCoroutine(ChromaticValue());
    }

    //PARA O DRAGAO
    IEnumerator ChromaticValue()
    {
        //Condição para iniciar: StartCoroutine(ChromaticValue());
        for (float i = 0f; i < 61; i += 6)
        {
            chromaticVar.intensity.value = i / 100;
            yield return new WaitForSecondsRealtime(0.1f);
        }
        //Colocar aqui a condição de quando que o efeito vai acabar:
        //if(dragao parar de rugir) {
        for (float i = 60f; i > -1; i -= 6)
        {
            chromaticVar.intensity.value = i / 100;
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }
}
