using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthSliderControl : MonoBehaviour, IObserver, IDeathObserver
{
    public HealthControl _agentHealth;
    public Slider healthSlider;
    public Animator anim;

    public bool isASpecialCase;

    void Start()
    {
        healthSlider.value = healthSlider.maxValue;
        ObserveSubject();
    }
    public void OnNotifyDamage(float damage)
    {
        healthSlider.value -= damage;
        if(!isASpecialCase)
            anim.SetTrigger("Damage");

    }

    void ObserveSubject()
    {
        _agentHealth.AddDamageObserver(this);
        _agentHealth.AddDeathObserver(this);
    }

    public void OnNotifyDeath()
    {
        healthSlider.value = 0f;
        anim.SetTrigger("Die");

        //gameOverPanel.SetActive(true);
    }


}