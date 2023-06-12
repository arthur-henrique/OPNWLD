using UnityEngine;

public class BossChaseState : BossState
{
    private float detectionDistance = 150f; // Adjust the value as per your requirement
    private float desiredDistance = 30f; // Adjust the value as per your requirement

    private bool canShoot;
    public BossChaseState(BossyAI bossAI) : base(bossAI)
    {
    }
    
    public override void UpdateState()
    {
        float distanceToPlayer = Vector3.Distance(bossAI.transform.position, bossAI.player.position);

        if (distanceToPlayer > detectionDistance)
        {
            canShoot = false;
            bossAI.canLeap = true;

            bossAI.TransitionToState(new BossIdleState(bossAI));
            bossAI.anim.SetBool("isPatrolling", true);

            return;
        }

        if (distanceToPlayer > desiredDistance)
        {
            canShoot = false;
            bossAI.isTooClose = false;
            bossAI.canLeap = true;


            // Calculate the direction towards the player
            Vector3 direction = bossAI.player.position - bossAI.transform.position;
            direction.Normalize();

            // Move the boss towards the player
            bossAI.agent.Move(bossAI.movementSpeed * Time.deltaTime * direction);
            bossAI.anim.SetBool("isPatrolling", true);
        }

        if (distanceToPlayer <= desiredDistance && distanceToPlayer >= bossAI.chaseDistanceThreshold)
        {
            canShoot = true;
            bossAI.isTooClose = false;

            bossAI.anim.SetBool("isPatrolling", false);
        }
        // Add chasing behavior here
        // Example: Move towards the player while maintaining a certain distance
        // Transition to the RangedAttack state when the boss is ready to attack
        if (bossAI.ReadyToAttack() && canShoot)
        {
            bossAI.TransitionToState(new BossRangedAttackState(bossAI));
            bossAI.canLeap = true;

        }
        // Transition to the MeleeAttack state when the player gets too close
        else if (Vector3.Distance(bossAI.transform.position, bossAI.player.position) < bossAI.meleeDistanceThreshold)
        {
            bossAI.isTooClose = true;
            Vector3 direction = bossAI.player.position - bossAI.transform.position;
            direction.Normalize();

            if(Vector3.Distance(bossAI.transform.position, bossAI.player.position) > 5f)
            {
                bossAI.agent.Move(bossAI.movementSpeed * Time.deltaTime * direction);
            }
            bossAI.TransitionToState(new BossMeleeAttackState(bossAI));

        }
    }
}
