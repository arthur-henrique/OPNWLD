using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IObserver
{
    public void OnNotifyDamage(float damage)
    {
        throw new System.NotImplementedException();
    }

    public void OnNotifyDeath()
    {
        throw new System.NotImplementedException();
    }
}
