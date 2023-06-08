using UnityEngine;

public class BossRangedAttackState : BossState
{
    private bool alternateShot = false;
    public BossRangedAttackState(BossyAI bossAI) : base(bossAI)
    {
    }

    public override void UpdateState()
    {
        // Add ranged attack behavior here
        // Example: Shoot projectiles at the player
        if (!alternateShot)
        {
            // Shoot projectile towards the player's direction
            Vector3 playerDirection = bossAI.player.position - bossAI.transform.position;
            bossAI.ShootProjectile(playerDirection);
        }
        else
        {
            // Shoot two projectiles slightly in front and behind the player's position
            Vector3 playerPosition = bossAI.player.position;
            Vector3 offset = bossAI.transform.right; // Offset direction from boss's right side

            bossAI.ShootProjectile(playerPosition + offset);
            bossAI.ShootProjectile(playerPosition - offset);

            // Transition back to the Chase state when the attack is complete
            if (bossAI.AttackComplete())
            {
                // Toggle the alternate shot flag for the next attack
                alternateShot = !alternateShot;

                bossAI.TransitionToState(new BossChaseState(bossAI));
            }
        }
    }
}
