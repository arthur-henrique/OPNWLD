using UnityEngine;

public class BossRangedAttackState : BossState
{
    public BossRangedAttackState(BossyAI bossAI) : base(bossAI)
    {
    }

    public override void UpdateState()
    {
        if(!bossAI.isAttacking)
        {
            Vector3 yCompensation = new Vector3(0f, 1f, 0f);
            bossAI.isAttacking = true;

            // Add ranged attack behavior here
            // Example: Shoot projectiles at the player
            if (!bossAI.alternateShot)
            {
                // Shoot projectile towards the player's direction
                Vector3 playerDirection = bossAI.player.position + yCompensation;
                //Debug.Log(playerDirection);
                bossAI.ShootProjectile(playerDirection);
            }
            else
            {
                // Shoot two projectiles slightly in front and behind the player's position
                Vector3 playerPosition = bossAI.player.position + yCompensation;
                Vector3 offset = bossAI.transform.right; // Offset direction from boss's right side
                //Debug.Log("duo");

                bossAI.ShootProjectile(playerPosition + offset);
                bossAI.ShootProjectile(playerPosition - offset);

            }

            
        }

        // Transition back to the Chase state when the attack is complete
        if (bossAI.AttackComplete())
        {
            // Toggle the alternate shot flag for the next attack
            bossAI.alternateShot = !bossAI.alternateShot;

            bossAI.TransitionToState(new BossChaseState(bossAI));
            bossAI.isAttacking = false;

        }

    }
}
