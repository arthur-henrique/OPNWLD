using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IPooledObject
{
    public float damage;

    public float force = 2f;
    public float DealPain()
    {
        gameObject.SetActive(false);
        return damage;
    }

    public void OnObjectSpawn()
    {
        Vector3 forceToApply = new Vector3(0, 0, force);
        GetComponent<Rigidbody>().velocity = forceToApply;
    }
}
