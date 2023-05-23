using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossIA : MonoBehaviour
{

    public enum States
    {
        idle,
        walk,
        patrol,
        attack
    }
        
    [SerializeField]
    private States states = States.idle;

    //geral
    public Animator magoAnimator;
    public Animator dragaoAnimator;
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
    public Transform[] walkPoints;
    public float vezesParaAtirar;
    bool resetarVezesParaAtirar;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        magoAnimator.SetBool("isInvoking", true);
        ChangeState(States.idle);
    }
  

     

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
       

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
            yield return new WaitForSeconds(3f);
            ChangeState(States.patrol);

        }



    }

    IEnumerator Patroling()
    {



        if (!walkPointSet && podePatrulhar) SearchWalkPoint();
        Debug.Log(walkPointSet);
        Debug.Log(walkpoint);
        
        if (walkPointSet)
        {
            agent.SetDestination(walkpoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkpoint;
        if (distanceToWalkPoint.magnitude < 1f)
        {
            ChangeState(States.attack);
            dragaoAnimator.SetBool("isPatrolling", false);
            walkPointSet = false;
        }
        if (!podePatrulhar)
        {
            yield return new WaitForSeconds(3f);
            podePatrulhar = true;
        }





        yield return new WaitForEndOfFrame();
        ChangeState(States.patrol);










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
            dragaoAnimator.SetBool("isPatrolling", true);
        }
    }

    IEnumerator Chase()
    {
        while (true)
        {

            //animator.SetBool("isChasing", true);
            agent.SetDestination(player.position);
            yield return new WaitForEndOfFrame();
          



        }
    }






    IEnumerator Attack()
    {

        

        if (!resetarVezesParaAtirar)
        {
            float randomNumber = Random.Range(1f, 3f);
            vezesParaAtirar = randomNumber;
            resetarVezesParaAtirar = true;
        }

        dragaoAnimator.SetBool("flameAttack", true);
        agent.SetDestination(transform.position);
        transform.LookAt(player);
        Debug.Log(alreadyAttacked);
        if (!alreadyAttacked && vezesParaAtirar >= 0)
        {
            Rigidbody rb = Instantiate(projectile, firePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            alreadyAttacked = true;
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * -1f, ForceMode.Impulse);
        }
        if(vezesParaAtirar == 0)
        {
            resetarVezesParaAtirar = false;
            ChangeState(States.patrol);
        }



        if (alreadyAttacked)
        {
            yield return new WaitForSeconds(timeBetweenAttacks);
            dragaoAnimator.SetBool("flameAttack", false);
            alreadyAttacked = false;
            vezesParaAtirar--;
        }


        yield return new WaitForEndOfFrame();
        ChangeState(States.attack);




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
