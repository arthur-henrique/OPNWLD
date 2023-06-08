using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGen : MonoBehaviour
{
    // Variáveis públicas para os tipos de projéteis e posições de lançamento.
    ObjectPooling objPooler;
    Quaternion randomRot;
    public string pool;

    // Método chamado quando o objeto é iniciado.
    void Start()
    {
        // Obtém uma instância da classe de pooling de objetos.
        objPooler = ObjectPooling.Instance;
    }

    // Método chamado a cada frame fixo.

    public void OnShoot(Vector3 whereToSpawn, Vector3 forward, bool hasHit)
    {
        randomRot = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
        objPooler.spawnFromPool(pool, whereToSpawn, forward, hasHit, randomRot);
    }
}
