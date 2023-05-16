using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSliderControl : MonoBehaviour, IObserver, IDeathObserver
{
    public HealthControl _agentHealth;
    public Slider healthSlider;

    void Start()
    {
        healthSlider.value = healthSlider.maxValue;
        ObserveSubject();
    }
    public void OnNotifyDamage(float damage)
    {
        healthSlider.value -= damage;
    }

    void ObserveSubject()
    {
        _agentHealth.AddDamageObserver(this);
        _agentHealth.AddDeathObserver(this);
    }

    public void OnNotifyDeath()
    {
        healthSlider.value = 0f;
        //gameOverPanel.SetActive(true);
    }

    
}