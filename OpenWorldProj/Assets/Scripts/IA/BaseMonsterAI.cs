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
    bool inCooldown;
    public GameObject pedra;
    public Vector3 walkpoint;
    public bool walkPointSet;
    [SerializeField]
    bool podePatrulhar = true;
    public GameObject spawnDoItem;
    public CapsuleCollider capsule;
    

     


    public GameObject itemDrop;

    public void Shoot()
    {
      Rigidbody rb = Instantiate(projectile, projectilePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward*30, ForceMode.Impulse);
        rb.AddForce(transform.up*-2, ForceMode.Impulse);

    }
    public void Spawnar()
    {
        if (!inCooldown)
        {
             Instantiate(projectile, projectilePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            inCooldown = true;
            StartCoroutine(CollingDown());
        }
    }
    IEnumerator CollingDown()
    {
        yield return new WaitForSeconds(4f);
        inCooldown = false;
    }
       

    public void AtivarPedra()
    {
        pedra.SetActive(true);
    }

    public void DesativarPedra()
    {
        pedra.SetActive(false);
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
        yield return new WaitForSeconds(0.5f);
       if(itemDrop != null)
        {

         Instantiate(itemDrop, spawnDoItem.transform.position, gameObject.transform.rotation);
        }

        
        Destroy(gameObject,0.4f);
            
    }

    public void AtivarCollider()
    {
        capsule.enabled = true;
    }

    public void DesativarCollider()
    {
        capsule.enabled = false;
    }
}







