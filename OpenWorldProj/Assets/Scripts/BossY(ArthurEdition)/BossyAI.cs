using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossyAI : MonoBehaviour, IObserver, IDeathObserver
{
    // Reference to the player
    [SerializeField] public Transform player;
    private Quaternion targetRotation;

    // Boss state machine
    private BossState currentState;

    // Animator
    public Animator anim;

    // Renderer
    public SkinnedMeshRenderer meshRenderer;
    public Texture normalAlbedo, redAlbedo;

    // NavMesh
    public NavMeshAgent agent;
    public float movementSpeed;
    private float rotationSpeed = 3f;

    // Health Control
    public HealthControl _agentHealth;

    // Cooldowns
    [SerializeField] float attackCooldownTime;
    [SerializeField] float attackCooldownDuration;

    // Timers
    [SerializeField] float attackTimer;
    [SerializeField] float attackDuration;

    // Checkers
    public bool isAttacking;
    public bool alternateShot = false;
    public bool canShoot;
    public bool isTooClose;
    public bool canLeap;
    public bool canBite;
    public bool canRotate;


    // Distances
    public float chaseDistanceThreshold = 20f; // Adjust the value as needed
    public float meleeDistanceThreshold = 15f; // Adjust the value as needed
    public float meleeAttackRange = 5f;
    public float detectionDistance = 150f;

    // Reference to where to shoot from
    [SerializeField] ObjectGen objectGen;
    public Transform projectileSpawnPoint; // Transform representing the spawn point of the projectile

    // ConeAttack Damage
    public MeshCollider coneMesh;
    public float coneDamage;

    // Leap Cube Attack
    public BoxCollider cubeMesh;
    public float leapDamage;

    //Materials
    public Material DragonsMaterial;

    // Self Collider
    public CapsuleCollider bossCollider;

    private void Start()
    {
        StartCoroutine(Introduction());
        // Initialize the boss AI with the Idle state
        agent = GetComponent<NavMeshAgent>();
        movementSpeed = agent.speed;
        ObserveSubject();

        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        // Update the current state
        currentState.UpdateState();
        attackTimer += Time.deltaTime;
    }

    private void LateUpdate()
    {
        // Ensure that the target is not null
        if (player != null)
        {
            // Calculate the direction from the current position to the target position
            Vector3 direction = player.position - transform.position;
            Vector3 correctedDirection = new Vector3(direction.x, 0f, direction.z);
            if (!isTooClose)
            {
                transform.rotation = Quaternion.LookRotation(correctedDirection);
            }
            else if(isTooClose && canRotate)
            {
                targetRotation = Quaternion.LookRotation(correctedDirection);
                StartCoroutine(DelayedRotation());
            }
            //Vector3 whereToLook = Vector3.Lerp(transform.forward, correctedDirection, 1.5f * Time.deltaTime);

            // Rotate the object to face the target
            
        }
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

    public void ShootProjectile(Vector3 direction, Vector3 directionTwo)
    {
        // Instantiate a projectile from the pool at the spawn point position and rotation
        StartCoroutine(Shoot(direction, directionTwo));
    }

    IEnumerator Shoot(Vector3 dir, Vector3 dirTwo)
    {
        yield return new WaitForSeconds(0.35f);

        if (!alternateShot)
        {
            objectGen.OnShoot(projectileSpawnPoint.position, dir, true);
            alternateShot = !alternateShot;
        }
        else
        {
            objectGen.OnShoot(projectileSpawnPoint.position, dir, true);
            objectGen.OnShoot(projectileSpawnPoint.position, dirTwo, true);
            alternateShot = !alternateShot;

        }

    }


    //private void OnDrawGizmos()
    //{
    //    // Draw the cone shape using Gizmos
    //    Vector3 direction = transform.forward;
    //    Vector3 origin = transform.position;
    //    float halfConeAngle = 45f * 0.5f;
    //    float coneHeight = meleeAttackRange;

    //    Vector3 coneBasePointA = origin + Quaternion.Euler(0f, -halfConeAngle, 0f) * direction * coneHeight;
    //    Vector3 coneBasePointB = origin + Quaternion.Euler(0f, halfConeAngle, 0f) * direction * coneHeight;

    //    Debug.Log("IsDrawingGismos");
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(origin, coneBasePointA);
    //    Gizmos.DrawLine(origin, coneBasePointB);
    //    Gizmos.DrawLine(coneBasePointA, coneBasePointB);
    //}

    IEnumerator Introduction()
    {
        anim.SetBool("scream", true);
        yield return new WaitForSeconds(1f);
        anim.SetBool("scream", false);
        yield return new WaitForSeconds(1.5f);
        bossCollider.enabled = true;
        currentState = new BossIdleState(this);

    }

    IEnumerator ReturnToNomal()
    {
        yield return new WaitForSeconds(0.25f);
        meshRenderer.material.SetTexture("_BaseColorMap", normalAlbedo);
        meshRenderer.UpdateGIMaterials();

    }

    public void ColorChange()
    {
        StartCoroutine(ReturnToNomal());
    }
    public void ActivateCone()
    {
        coneMesh.enabled = true;
        canRotate = false;
    }
    public void DeactivateCone()
    {
        coneMesh.enabled = false;
        canRotate = true;
    }

    public void ActivateLeapCube()
    {
        cubeMesh.enabled = true;
        canRotate = false;
    }
    public void DeactivateLeapCube()
    {
        cubeMesh.enabled = false;
        canRotate = true;
        StartCoroutine(DelayToBite());

    }

    void ObserveSubject()
    {
        _agentHealth.AddDamageObserver(this);
        _agentHealth.AddDeathObserver(this);
    }
    private IEnumerator DelayedRotation()
    {
        canRotate = false;

        yield return new WaitForSeconds(1.2f);

        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.5f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        canRotate = true;
    }
    private IEnumerator DelayToBite()
    {
        yield return new WaitForSeconds(0.35f);
        canBite = true;
    }

    public void OnNotifyDamage(float damage)
    {
        StartCoroutine(ChangeColorDelay());
    }

    /// <summary>
    /// Changes the Dragon's material color for damage
    /// </summary>
    /// <returns></returns>
    IEnumerator ChangeColorDelay()
    {
        for (int i = 0; i < 11; i++)
        {
            DragonsMaterial.SetFloat("_TurnRed", i / 10f);
            yield return new WaitForSecondsRealtime(0.014f);
        }
        yield return new WaitForSecondsRealtime(0.08f);
        for (int i = 9; i > -1; i--)
        {
            DragonsMaterial.SetFloat("_TurnRed", i / 10f);
            yield return new WaitForSecondsRealtime(0.014f);
        }
    }



    public void OnNotifyDeath()
    {
        bossCollider.isTrigger = false;
        Destroy(_agentHealth);
        anim.SetTrigger("Die");
        DeactivateCone();
        DeactivateLeapCube();
        this.enabled= false;
    }

    public void OnNotifyHeal(float heal)
    {
        throw new System.NotImplementedException();
    }
}
