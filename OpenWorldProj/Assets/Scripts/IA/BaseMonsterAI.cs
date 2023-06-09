using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseMonsterAI : MonoBehaviour
{
    public float velocidadeDePatrulha;
    public GameObject projectile;
    public Transform projectilePoint;
    public List<Transform> wayPoints;
    NavMeshAgent agent;
    public float walkPointRange;
    public LayerMask whatIsGround;

    public Vector3 walkpoint;
    public bool walkPointSet;
    [SerializeField]
    bool podePatrulhar = true;
    public GameObject spawnDoItem;


    public GameObject itemDrop;

    public void Shoot()
    {
      Rigidbody rb =   Instantiate(projectile, projectilePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(PlayerManager.instance.player.transform.position , ForceMode.Impulse);
        rb.AddForce(PlayerManager.instance.player.transform.position *10, ForceMode.Impulse);
    }

    public void EscolherPontoPatrol()
    {
            agent = gameObject.GetComponent<NavMeshAgent>();
            agent.speed = velocidadeDePatrulha;
        if (podePatrulhar)
        {
            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
            podePatrulhar = false;
        }
        else
        {
            StartCoroutine(ResetarPodePatrulhar());
        }
    
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
        }

    }
    IEnumerator ResetarPodePatrulhar()
    {
        yield return new WaitForSeconds(10f);
        podePatrulhar = true;
    }

    public void Death()
    {
        StartCoroutine(Destruir());
    }
    IEnumerator Destruir()
    {
        yield return new WaitForSeconds(1.5f);
       
         Instantiate(itemDrop, spawnDoItem.transform.position, gameObject.transform.rotation);
        
        Destroy(gameObject,0.4f);
            
    }

}







