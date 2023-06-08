using UnityEngine;

public class BossIdleState : BossState
{
    public BossIdleState(BossyAI bossAI) : base(bossAI)
    {
    }

    public override void UpdateState()
    {
        // Add any idle behavior here
        // Example: Look around, perform idle animations, etc.

        // Transition to the Chase state when the player is within a certain distance
        if (Vector3.Distance(bossAI.transform.position, bossAI.player.position) < bossAI.chaseDistanceThreshold)
        {
            bossAI.TransitionToState(new BossChaseState(bossAI));
        }
    }
}
