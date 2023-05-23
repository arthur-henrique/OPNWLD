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
    private SphereCollider collider;
    public Vector3 target { get;  set; }
    public bool hit { get; set; }

    [SerializeField]
    private float damage;

    void Start()
    {
        projectileAnim = GetComponent<Animator>();
        collider = GetComponent<SphereCollider>();
    }
    public void OnObjectSpawn(Vector3 forward, bool hasHit)
    {
        if(!hasHit)
            GetComponent<Rigidbody>().AddForce(forward * projectileSpeed/75, ForceMode.Impulse);
        target = forward;
        hit = hasHit;
        StartCoroutine(Deactivate());
    }
    void Update()
    {
        if(hit)
            transform.position = Vector3.MoveTowards(transform.position, target, projectileSpeed * Time.deltaTime);

        if(!hit)
        {
            //projectileAnim.SetBool("shrink", true);
            collider.isTrigger = false;
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
