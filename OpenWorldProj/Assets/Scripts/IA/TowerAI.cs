using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAI : MonoBehaviour
{
 


    //geral
    public Animator animator;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    public Transform firePoint;

    public float timeBetweenAttacks;
    public GameObject projectile;





    public float attackRange;
    public bool playerInSightRange, playerInAttackRange;
    private bool alreadyAttacked;
    public float walkPointRange;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
       
    }


    private void Update()
    {

        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (playerInAttackRange)
        {
            Attack();
        }
    }



    void Attack()
    {


        

            transform.LookAt(player);

            if (!alreadyAttacked)
            {
                //animator.SetBool("isAttacking", true);
                Rigidbody rb = Instantiate(projectile, firePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
                rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
                rb.AddForce(transform.up * -1f, ForceMode.Impulse);
                alreadyAttacked = true;
                StartCoroutine(ResetAttack());


            }



        


    }
    IEnumerator ResetAttack()
    {
        //animator.SetBool("isAttacking", false);
        yield return new WaitForSeconds(timeBetweenAttacks);
        alreadyAttacked = false;
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);


    }
}
