using UnityEngine;

public class ConeCollider : MonoBehaviour, IDealDamage
{
    public float damage;
    public float DealDamage()
    {
        return damage;
    }
}