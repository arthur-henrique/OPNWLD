using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    [SerializeField] HealthControl bossHealth;
    [SerializeField] GameObject enemyPrefabs;
    [SerializeField] GameObject tougherEnemyPrefabs;
    public Transform[] spawnTransforms;
    public float instanciateCooldown;
    public float tougherInstanciateCooldown;

    private void Start()
    {
        StartCoroutine(HealthChecker());   
    }
    public void SpawnEnemies()
    {
        if(bossHealth.health < bossHealth.health *.6f)
        {
            int spawnSpotOne = Random.Range(0, spawnTransforms.Length);
            int spawnSpotTwo = Random.Range(0, spawnTransforms.Length);
            if(spawnSpotOne == spawnSpotTwo)
            {
                if(spawnSpotTwo + 1 < spawnTransforms.Length)
                {
                    spawnSpotTwo++;
                }
                else
                {
                    spawnSpotTwo--;
                }
            }
            StartCoroutine(BasicSummon(spawnSpotOne, spawnSpotTwo));
        }

        if(bossHealth.health < bossHealth.health * .3f)
        {
            int spawnSpotOne = Random.Range(0, spawnTransforms.Length);
            StartCoroutine(ToughSummon(spawnSpotOne));
        }
    }
    
    IEnumerator HealthChecker()
    {
        yield return new WaitForSeconds(0.2f);
        SpawnEnemies();
        StartCoroutine(HealthChecker());
    }
    IEnumerator BasicSummon(int spotOne, int spotTwo)
    {
        yield return new WaitForSeconds(instanciateCooldown);
        Instantiate(enemyPrefabs, spawnTransforms[spotOne].position, Quaternion.identity);
        Instantiate(enemyPrefabs, spawnTransforms[spotTwo].position, Quaternion.identity);
    }
    IEnumerator ToughSummon(int spot)
    {
        yield return new WaitForSeconds(tougherInstanciateCooldown);
        Instantiate(tougherEnemyPrefabs, spawnTransforms[spot].position, Quaternion.identity);

    }


}
