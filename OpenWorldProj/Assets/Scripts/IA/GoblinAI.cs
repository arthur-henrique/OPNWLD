using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinAI : MonoBehaviour
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
    public bool walkPointSet;


    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    private bool alreadyAttacked;
    public float walkPointRange;
    [SerializeField]
    bool podePatrulhar;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(Idle());
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
        while (true)
        {
            //animator.SetBool("isPatrolling", false);
            yield return new WaitForEndOfFrame();
            ChangeState(States.patrol);

        }



    }

    IEnumerator Patroling()
    {



        if (!walkPointSet && podePatrulhar) SearchWalkPoint();
        Debug.Log(walkPointSet);
        Debug.Log(walkpoint);
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
        if (!podePatrulhar)
        {
            yield return new WaitForSeconds(3f);
            podePatrulhar = true;
        }



        StatesControl();

        yield return new WaitForEndOfFrame();
        //animator.SetBool("isPatrolling", false);










    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkpoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkpoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
            podePatrulhar = false;
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



        agent.SetDestination(transform.position);
        transform.LookAt(player);
        Debug.Log(alreadyAttacked);
        if (!alreadyAttacked)
        {
            animator.SetBool("isAttacking", true);

        }
          


        if (alreadyAttacked)
        {
            yield return new WaitForSeconds(timeBetweenAttacks);
            alreadyAttacked = false;
        }


        yield return new WaitForEndOfFrame();
        StatesControl();



    }




    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, walkPointRange);
    }
}
