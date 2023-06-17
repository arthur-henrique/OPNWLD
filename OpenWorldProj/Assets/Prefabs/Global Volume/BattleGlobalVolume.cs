using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BattleGlobalVolume : MonoBehaviour, IObserver, IDeathObserver
{

    private Volume volumeVar;
    private Bloom bloomVar;
    private Vignette vignetteVar;

    public HealthControl agentHealth;


    // Start is called before the first frame update
    void Start()
    {
        volumeVar = GetComponent<Volume>();
        volumeVar.profile.TryGet(out bloomVar);
        volumeVar.profile.TryGet(out vignetteVar);
        vignetteVar.intensity = new ClampedFloatParameter(0.5f,0f, 1f,true);
        AddObserver();
    }

    [ContextMenu("test")]
    private void Test()
    {
        bloomVar.scatter.value = 0.1f;
        vignetteVar.intensity.value = 0.5f; //tem que ter as opções já habilitadas anteriormente
    }

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

    }

    public void OnNotifyHeal(float heal)
    {

    }
}
