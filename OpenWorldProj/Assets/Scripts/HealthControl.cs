 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthControl : ObservableSubject
{
    [SerializeField] private bool isPlayer = false;
    public float health = 100f;
    public GameObject canvasMorte;
    public bool isDead;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out IDealDamage damageDealer))
        {
            if(!isPlayer && !isDead && other.gameObject.layer == 9)
            {
                Debug.LogWarning("EnemyShot");
                float damage = damageDealer.DealDamage();
                health -= damage;
                if (health > 0)
                {
                    NotifyDamage(damage);
                }
                else if (health <= 0f)
                {
                    isDead = true;
                    NotifyDeath();
                }
            }
            else if (isPlayer && !isDead && other.gameObject.layer == 10)
            {
                Debug.LogWarning("PlayerShot");
                float damage = damageDealer.DealDamage();
                health -= damage;
                if (health > 0)
                {
                    NotifyDamage(damage);
                }
                else if (health <= 0f)
                {
                    isDead = true;
                    NotifyDeath();
                    canvasMorte.SetActive(true);

                }
                  
            }
            //else
            //{
            //    other.gameObject.SetActive(false);
            //}
        }

    }
}
