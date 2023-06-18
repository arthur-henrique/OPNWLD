using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GlobalVolumeScript : MonoBehaviour, IObserver, IDeathObserver
{

    private Volume volumeVar;
    //private Bloom bloomVar;
    private Vignette vignetteVar;
    static float t = 0.0f;

    public HealthControl agentHealth;

    void Start()
    {
        volumeVar = GetComponent<Volume>();
        //volumeVar.profile.TryGet(out bloomVar);
        volumeVar.profile.TryGet(out vignetteVar);
        //vignetteVar.intensity = new ClampedFloatParameter(0.5f,0f, 1f,true);
        StartCoroutine(FindPlayer());
    }

    void Update()
    {
        vignetteVar.intensity.value = Mathf.Lerp(0, 0.455f, t);
        
        if (t < 1.0f)
        {
        t += 2.3f * Time.deltaTime;
        }
        //vignetteVar.intensity.value = 0.455f;

    }

    /*[ContextMenu("test")]
    private void Test()
    {
        bloomVar.scatter.value = 0.1f;
        vignetteVar.intensity.value = 0.5f; //tem que ter as opções já habilitadas anteriormente
    }*/

    void AddObserver()
    {
        agentHealth.AddDamageObserver(this);
        agentHealth.AddDeathObserver(this);
    }


    public void OnNotifyDeath()
    {

    }

    public void OnNotifyDamage(float damage)
    {
        print("levou dano");
    }

    public void OnNotifyHeal(float heal)
    {

    }

    IEnumerator FindPlayer()
    {
        yield return new WaitForSeconds(0.15f);
        agentHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthControl>();
        AddObserver();

    }
}
