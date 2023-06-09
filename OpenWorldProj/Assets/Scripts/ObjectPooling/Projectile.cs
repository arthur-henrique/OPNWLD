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
    public Vector3 target { get;  set; }
    private Vector3 beyond;
    public bool hit { get; set; }

    [SerializeField]
    private float damage;

    void Start()
    {
        projectileAnim = GetComponent<Animator>();
        spherecollider = GetComponent<SphereCollider>();
        beyond = new (2f, 1f, 2f);
    }
    public void OnObjectSpawn(Vector3 forward, bool hasHit)
    {
        if(!hasHit)
            GetComponent<Rigidbody>().AddForce(forward * projectileSpeed/55, ForceMode.Impulse);
        target = forward;
        hit = hasHit;
        StartCoroutine(Deactivate());
    }
    void Update()
    {
        if (hit)
        {
            Vector3 currentPosition = transform.position;
            Vector3 newPosition = currentPosition + (target - currentPosition).normalized * projectileSpeed * Time.deltaTime;

            RaycastHit raycastHit;
            if (Physics.Linecast(currentPosition, newPosition, out raycastHit))
            {
                // Calculate the reflection vector based on the surface normal of the hit
                Vector3 reflectionDirection = Vector3.Reflect(newPosition - currentPosition, raycastHit.normal);

                // Move the projectile along the reflection direction
                newPosition = raycastHit.point + reflectionDirection.normalized * projectileSpeed * Time.deltaTime;
            }

            transform.position = newPosition;
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
