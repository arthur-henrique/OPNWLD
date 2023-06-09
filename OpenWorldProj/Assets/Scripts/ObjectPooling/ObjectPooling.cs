using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
    // Uma classe para representar cada pool de objetos
    [System.Serializable]
    public class Pool
    {
        public string tag; // O nome que identifica o pool
        public GameObject prefab; // O objeto que será armazenado no pool
        public int poolSize; // O tamanho máximo do pool
    }

    #region Singleton
    // O padrão Singleton garante que só haja uma instância deste objeto em execução
    public static ObjectPooling Instance;

    // Configuração do Singleton
    private void Awake()
    {
        Instance = this;
    }
    #endregion;

    public List<Pool> pools; // Uma lista de todos os pools a serem criados

    // Um dicionário que mapeia as tags para as filas de objetos
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            // Cria a fila de objetos
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // Preenche a fila com objetos
            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                if(pool.tag == "PlayerArrow")
                    obj.transform.SetParent(GameObject.Find("PlayerProjectiles").transform);
                objectPool.Enqueue(obj);

            }

            // Adiciona a fila ao dicionário usando a tag como chave
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject spawnFromPool(string tag, Vector3 position, Vector3 forward, bool hasHit, Quaternion rotation)
    {
        // Se não existe uma fila com a tag dada, retorna null
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag: " + tag + " doens't exist. ");
            return null;
        }

        // Pega o objeto da fila
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();
        objectToSpawn.GetComponent<Rigidbody>().velocity = Vector3.zero;
        objectToSpawn.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        objectToSpawn.GetComponent<Rigidbody>().useGravity = false;
        objectToSpawn.GetComponent<Projectile>().hasReach = false;

        // Ativa o objeto
        objectToSpawn.SetActive(true);

        // Define a posição e a rotação do objeto
        objectToSpawn.transform.position = new Vector3(position.x, position.y, position.z);
        objectToSpawn.transform.rotation = rotation;

        // Verifica se o objeto implementa a interface IPooledObject e chama seu método OnObjectSpawn
        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>();
        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn(forward, hasHit);
        }

        // Adiciona o objeto de volta na fila
        poolDictionary[tag].Enqueue(objectToSpawn);

        // Retorna o objeto spawnado
        return objectToSpawn;
    }
}
