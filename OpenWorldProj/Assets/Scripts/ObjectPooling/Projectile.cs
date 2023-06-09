using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour, IPooledObject, IDealDamage
{
    [SerializeField]
    private float projectileSpeed = 25f;
    [SerializeField]
    private float timeToDeactivate = 5f;
    private Animator projectileAnim;
    private SphereCollider spherecollider;
    private Rigidbody body;
    public Vector3 target { get;  set; }
    private Vector3 beyond;
    public bool hit { get; set; }

    [SerializeField]
    private float damage;
    public bool hasReach = false;
    void Start()
    {
        body = GetComponent<Rigidbody>();
        hasReach = false;
        projectileAnim = GetComponent<Animator>();
        spherecollider = GetComponent<SphereCollider>();
        beyond = new (2f, 1f, 2f);
        
    }
    public void OnObjectSpawn(Vector3 forward, bool hasHit)
    {
        if(!hasHit)
            body.AddForce(forward * projectileSpeed/55, ForceMode.Impulse);
        target = forward;
        hit = hasHit;
        StartCoroutine(Deactivate());
    }
    void Update()
    {

        // Ensure that the target is not null
        if (target != null)
        {
            // Calculate the direction from the current position to the target position
            Vector3 direction = target - transform.position;

            // Rotate the object to face the target
            transform.rotation = Quaternion.LookRotation(direction);
        }
    
        if (hit)
        {
            Vector3 currentPosition = transform.position;
            Vector3 newPosition = currentPosition + (target - currentPosition).normalized * projectileSpeed * Time.deltaTime;

            if (Vector3.Distance(currentPosition, target) <= (projectileSpeed * Time.deltaTime))
            {
                hasReach = true;
                // The projectile has reached or surpassed the target position
                body.useGravity = true;
                body.AddForce(transform.forward, ForceMode.Impulse);
            }

            if(!hasReach)
            {
                transform.position = newPosition;
            }
        }

        if (!hit)
        {
            //projectileAnim.SetBool("shrink", true);
            spherecollider.isTrigger = false;
            if(Vector3.Distance(transform.position, target) < 0.1f)
            gameObject.SetActive(false);
        }
    }

    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(timeToDeactivate );
        gameObject.SetActive(false);
    }

    public float DealDamage()
    {
        gameObject.SetActive(false);
        return damage;
    }
}
