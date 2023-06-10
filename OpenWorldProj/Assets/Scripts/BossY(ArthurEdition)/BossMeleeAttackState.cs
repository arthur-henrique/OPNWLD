using UnityEngine;

public class BossMeleeAttackState : BossState
{
    private float leapDistance = 10f; // Adjust the leap distance as per your requirement
    private float coneAngle = 45f; // Adjust the cone angle as per your requirement
    private bool hasPerformedLeapAttack = false;
    public float conedamage;

    public BossMeleeAttackState(BossyAI bossAI) : base(bossAI)
    {
    }

    public override void UpdateState()
    {
        if(!bossAI.isAttacking)
        {
            bossAI.isAttacking = true;
            // Add melee attack behavior here
            // Example: Perform melee attacks on the player
            if (!hasPerformedLeapAttack)
            {
                PerformLeapAttack();
                hasPerformedLeapAttack = true;
            }

            DealConeDamage();

            // Additional melee attack logic

            // Transition back to the Chase state when the attack is complete
            
        }
        if (bossAI.AttackComplete())
        {
            bossAI.TransitionToState(new BossChaseState(bossAI));
            bossAI.isAttacking = false;
        }
    }

    private void PerformLeapAttack()
    {
        // Implement your leap attack logic here
        // This can include moving the boss towards the player with a specific leap distance
        // You can use the bossAI.agent.Move() method or any other movement logic you prefer
        Debug.Log("Leap");
    }

    private void DealConeDamage()
    {
        bossAI.anim.SetTrigger("bite");
        //// Get all colliders within a cone-shaped area in front of the boss
        //Collider[] hitColliders = Physics.OverlapSphere(bossAI.transform.position, bossAI.meleeAttackRange);
        //foreach (Collider collider in hitColliders)
        //{
        //    // Calculate the angle between the boss's forward direction and the collider's position
        //    Vector3 directionToCollider = collider.transform.position - bossAI.transform.position;
        //    float angle = Vector3.Angle(bossAI.transform.forward, directionToCollider);

        //    // If the collider is within the cone angle, deal damage to it
        //    if (angle <= coneAngle)
        //    {
        //        // Implement your damage logic here
        //        // You can call your existing function that deals damage to the collider
        //        // Make sure the function is accessible from this script
        //        if (collider.CompareTag("Player"))
        //        {
        //            Debug.Log("ConeAttack");
        //        }
        //    }
        //}

        
    }

    private void OnDrawGizmos()
    {
        // Draw the cone shape using Gizmos
        Vector3 direction = bossAI.transform.forward;
        Vector3 origin = bossAI.transform.position;
        float halfConeAngle = coneAngle * 0.5f;
        float coneHeight = bossAI.meleeAttackRange;

        Vector3 coneBasePointA = origin + Quaternion.Euler(0f, -halfConeAngle, 0f) * direction * coneHeight;
        Vector3 coneBasePointB = origin + Quaternion.Euler(0f, halfConeAngle, 0f) * direction * coneHeight;

        Debug.Log("IsDrawingGismos");
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, coneBasePointA);
        Gizmos.DrawLine(origin, coneBasePointB);
        Gizmos.DrawLine(coneBasePointA, coneBasePointB);
    }

    


}
