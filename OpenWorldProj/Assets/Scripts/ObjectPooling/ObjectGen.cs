using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGen : MonoBehaviour
{
    // Vari�veis p�blicas para os tipos de proj�teis e posi��es de lan�amento.
    ObjectPooling objPooler;
    Quaternion randomRot;
    public string pool;

    // M�todo chamado quando o objeto � iniciado.
    void Start()
    {
        // Obt�m uma inst�ncia da classe de pooling de objetos.
        objPooler = ObjectPooling.Instance;
    }

    // M�todo chamado a cada frame fixo.

    public void OnShoot(Vector3 whereToSpawn, Vector3 forward, bool hasHit)
    {
        randomRot = Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f));
        objPooler.spawnFromPool(pool, whereToSpawn, forward, hasHit, randomRot);
    }
}
