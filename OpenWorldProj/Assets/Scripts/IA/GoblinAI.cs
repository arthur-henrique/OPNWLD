using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinAI : MonoBehaviour
{
    public enum States
    {
        
        patrol,
        walk,
        attack
    }
    [SerializeField]
    private States states = States.patrol;

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
    public bool walkPointSet;


    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    private bool alreadyAttacked;
    public float walkPointRange;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(Patroling());
    }
    private void StatesControl()
    {
        StopAllCoroutines();
        if (playerInSightRange && !playerInAttackRange)
        {
            //animator.SetBool("isPatrolling", false);
            ChangeState(States.walk);
        }

        if (playerInSightRange && playerInAttackRange)
        {
            ChangeState(States.attack);
        }

        if (!playerInSightRange && !playerInAttackRange)
        {
            //animator.SetBool("isChasing", false);
            ChangeState(States.patrol);
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

   

    IEnumerator Patroling()
    {

        while (true)
        {

            if (!walkPointSet) StartCoroutine(SearchWalkPoint());

            //animator.SetBool("isPatrolling", true);
            if (walkPointSet)
            {
                agent.SetDestination(walkpoint);
            }

            Vector3 distanceToWalkPoint = transform.position - walkpoint;
            if (distanceToWalkPoint.magnitude < 1f)
            {
                walkPointSet = false;
            }



            yield return new WaitForSeconds(5f);
            StatesControl();
            //animator.SetBool("isPatrolling", false);


        }







    }

    IEnumerator SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkpoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        yield return new WaitForEndOfFrame();
        if (Physics.Raycast(walkpoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    IEnumerator Chase()
    {
        while (true)
        {

            //animator.SetBool("isChasing", true);
            agent.SetDestination(player.position);
            yield return new WaitForEndOfFrame();
            StatesControl();



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
                animator.SetBool("isAttacking", true);
                StartCoroutine(ResetAttack());



            }


            yield return new WaitForEndOfFrame();
            StatesControl();
        }


    }
    IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);
        animator.SetBool("isAttacking", false);
        alreadyAttacked = false;
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

    }
}
