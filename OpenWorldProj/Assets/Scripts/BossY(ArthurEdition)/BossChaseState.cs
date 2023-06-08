using UnityEngine;

public class BossChaseState : BossState
{
    public BossChaseState(BossyAI bossAI) : base(bossAI)
    {
    }

    public override void UpdateState()
    {
        // Add chasing behavior here
        // Example: Move towards the player while maintaining a certain distance

        // Transition to the RangedAttack state when the boss is ready to attack
        if (bossAI.ReadyToAttack())
        {
            bossAI.TransitionToState(new BossRangedAttackState(bossAI));
        }
        // Transition to the MeleeAttack state when the player gets too close
        else if (Vector3.Distance(bossAI.transform.position, bossAI.player.position) < bossAI.meleeDistanceThreshold)
        {
            bossAI.TransitionToState(new BossMeleeAttackState(bossAI));
        }
    }
}
