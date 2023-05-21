using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public enum States
    {
        idle,
        patrol,
        walk,
        attack
    }
    [SerializeField]
    private States states = States.idle;

    //geral
    public Animator animator;
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public Transform firePoint;

    public float timeBetweenAttacks;
    public GameObject projectile;


    //Patroling
    public Vector3 walkpoint;
    bool walkPointSet;


    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    private bool alreadyAttacked;
    public float walkPointRange;
    float randomNumber; 

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(Idle());
    }
    private void StatesControl()
    {
        if (!walkPointSet) SearchWalkPoint();
        if (playerInSightRange && !playerInAttackRange)
        {
            ChangeState(States.walk);
        }

        if (playerInSightRange && playerInAttackRange)
        {
            ChangeState(States.attack);
        }

        if (!playerInSightRange && !playerInAttackRange)
        {
            
            ChangeState(States.idle);
        }

    }
    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

    }



    public void ChangeState(States state)
    {
        states = state;
        StopAllCoroutines(); // Interrompe a co-rotina atual (estado atual).
        switch (states)
        {
            case States.idle:
                StartCoroutine(Idle());
                break;
               

            case States.patrol:
                StartCoroutine(Patroling());
                break;



            case States.walk:
                StartCoroutine(Chase());
                break;



            case States.attack:
                StartCoroutine(Attack());
                break;
        }
          
    }

    IEnumerator Idle()
    {

        //animator.SetFloat("Velocidade", 0);
            
            yield return new WaitForEndOfFrame();
            ChangeState(States.patrol);

        
    }

    IEnumerator Patroling()
    {
        
        
            walkPointSet = false;

            if (!walkPointSet) SearchWalkPoint();

            if (walkPointSet)
            {
                agent.SetDestination(walkpoint);
            }

            Vector3 distanceToWalkPoint = transform.position - walkpoint;
            if (distanceToWalkPoint.magnitude < 1f)
            {
                walkPointSet = false;
            }
            StatesControl();
            
            
            yield return new WaitForSeconds(3f);
            ChangeState(States.idle);
             
            
            
            

        
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkpoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkpoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    IEnumerator Chase()
    {
        while (true)
        {
            //animator.SetFloat("Velocidade", 1);
            agent.SetDestination(player.position);

            if (playerInAttackRange)
            {
                ChangeState(States.attack);
            }

            if (!playerInSightRange)
            {
                ChangeState(States.idle);
            }

            yield return new WaitForEndOfFrame();

        }
    }






    IEnumerator Attack()
    {

            
         while (true)
         {
            agent.SetDestination(transform.position);
            transform.LookAt(player);

            if (!alreadyAttacked)
            {
                Rigidbody rb = Instantiate(projectile, firePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
                rb.AddForce(transform.up * -1f, ForceMode.Impulse);

                alreadyAttacked = true;
                StartCoroutine(ResetAttack());
            }


            yield return new WaitForEndOfFrame();

         }


    }
   IEnumerator ResetAttack()
   {
     yield return new WaitForSeconds(timeBetweenAttacks);
     alreadyAttacked = false;

    if (!playerInAttackRange)
    {
         ChangeState(States.walk);
    }

   }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }
}


       



    





   









