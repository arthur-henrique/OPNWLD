using UnityEngine;

public class BossMeleeAttackState : BossState
{
    public BossMeleeAttackState(BossyAI bossAI) : base(bossAI)
    {
    }

    public override void UpdateState()
    {
        // Add melee attack behavior here
        // Example: Perform melee attacks on the player

        // Transition back to the Chase state when the attack is complete
        if (bossAI.AttackComplete())
        {
            bossAI.TransitionToState(new BossChaseState(bossAI));
        }
    }
}
