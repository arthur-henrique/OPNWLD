using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour, IDealDamage
{
    [SerializeField]
    public float damage;
    public CapsuleCollider capscollider;
    public float DealDamage()
    {
        return damage;
    }
}
