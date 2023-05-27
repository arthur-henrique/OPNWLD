using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IDealDamage
{
    [SerializeField]
    public float damage;
    public CapsuleCollider collider;
    public float DealDamage()
    {
        return damage;
    }

    public void ColliderSwap()
    {
        collider.isTrigger = !collider.isTrigger;
    }
}
