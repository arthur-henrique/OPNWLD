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

    public void Shoot()
    {
      Rigidbody rb =   Instantiate(projectile, projectilePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 30, ForceMode.Impulse);
        rb.AddForce(transform.up * 7, ForceMode.Impulse);
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
        Destroy(gameObject, 1.5f);
    }
    
}







