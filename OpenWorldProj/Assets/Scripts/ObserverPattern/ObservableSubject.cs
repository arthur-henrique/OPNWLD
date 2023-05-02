using System.Collections.Generic;
using UnityEngine;

public abstract class ObservableSubject : MonoBehaviour
{
    // a collection of all the observers of this subject
    private List<IObserver> _observers = new List<IObserver>();

    public void AddObserver(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }

    protected void NotifyDamage()
    {
        _observers.ForEach((_observer) => { _observer.OnNotifyDamage(); });
    }

    protected void NotifyDeath()
    {
        _observers.ForEach((_observer) => { _observer.OnNotifyDeath(); });
    }

}
