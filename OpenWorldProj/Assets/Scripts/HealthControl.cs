using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthControl : ObservableSubject
{
    public float health = 100f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out IDealDamage damageDealer))
        {
            Debug.LogWarning("Shot");
            float damage = damageDealer.DealDamage();
            health -= damage;
            if (health > 0)
            {
                NotifyDamage(damage);
            }
            else if (health <= 0f)
                NotifyDeath();
        }
    }
}
