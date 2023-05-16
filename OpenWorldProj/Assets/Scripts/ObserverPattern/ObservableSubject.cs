using System.Collections.Generic;
using UnityEngine;

public abstract class ObservableSubject : MonoBehaviour
{
    private List<IObserver> _damageObservers = new List<IObserver>();
    private List<IDeathObserver> _deathObservers = new List<IDeathObserver>();


    // Add the observer to the subject's collection
    public void AddDamageObserver(IObserver observer)
    {
        _damageObservers.Add(observer);
    }

    // Remove the observer to the subject's collection

    public void RemoveDamageObserver(IObserver observer)
    {
        _damageObservers.Remove(observer);
    }

    public void AddDeathObserver(IDeathObserver observer)
    {
        _deathObservers.Add(observer);
    }

    // Remove the observer to the subject's collection

    public void RemoveDeathObserver(IDeathObserver observer)
    {
        _deathObservers.Remove(observer);
    }

    // Notify each observer that an event has occurred
    protected void NotifyDamage(float damageToNotify)
    {
        _damageObservers.ForEach((_observer) =>
        {
            _observer.OnNotifyDamage(damageToNotify);
        });
    }

    protected void NotifyDeath()
    {
        _deathObservers.ForEach((_observer) =>
        {
            _observer.OnNotifyDeath();
        });
    }
}
