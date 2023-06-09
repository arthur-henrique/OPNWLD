using UnityEngine;
using UnityEngine.AI;

public class BossyAI : MonoBehaviour
{
    // Reference to the player
    [SerializeField] public Transform player;

    // Boss state machine
    [SerializeField]
    private BossState currentState;

    // NavMesh
    private NavMeshAgent navMeshAgent;

    // Cooldowns
    [SerializeField] float attackCooldownTime;
    [SerializeField] float attackCooldownDuration;

    // Timers
    [SerializeField] float attackTimer;
    [SerializeField] float attackDuration;

    // Checkers
    public bool isAttacking;
    public bool alternateShot = false;


    // Distances
    public float chaseDistanceThreshold = 10f; // Adjust the value as needed
    public float meleeDistanceThreshold = 2f; // Adjust the value as needed

    // Reference to where to shoot from
    [SerializeField] ObjectGen objectGen;
    public Transform projectileSpawnPoint; // Transform representing the spawn point of the projectile


    private void Start()
    {
        // Initialize the boss AI with the Idle state
        currentState = new BossIdleState(this);
        navMeshAgent = GetComponent<NavMeshAgent>();

        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        // Update the current state
        currentState.UpdateState();
        attackTimer += Time.deltaTime;

    }

    // Method to transition to a new state
    public void TransitionToState(BossState nextState)
    {
        currentState = nextState;
    }

    public bool ReadyToAttack()
    {
        // Check if the boss's attack cooldown has expired
        if (Time.time >= attackCooldownTime)
        {
            // Reset the attack cooldown time
            attackCooldownTime = Time.time + attackCooldownDuration;

            // Add any additional conditions for readiness, such as energy levels, ammunition, etc.
            // Example: if (bossAI.currentEnergy >= bossAI.attackEnergyCost)

            return true;
        }

        return false;
    }

    public bool AttackComplete()
    {
        // Check if the boss's attack action is complete
        if (attackTimer >= attackDuration)
        {
            // Reset the attack timer
            attackTimer = 0f;

            // Add any additional conditions for attack completion, such as animations or specific attack logic
            // Example: if (bossAI.projectileCount <= 0)

            return true;
        }

        return false;
    }

    public void ShootProjectile(Vector3 direction)
    {
        // Instantiate a projectile from the pool at the spawn point position and rotation
        objectGen.OnShoot(projectileSpawnPoint.position, direction, true);
    }

}
