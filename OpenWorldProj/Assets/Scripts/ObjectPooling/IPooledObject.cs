using UnityEngine;

public interface IPooledObject
{
    void OnObjectSpawn(Vector3 forward, bool hasHit);
}
