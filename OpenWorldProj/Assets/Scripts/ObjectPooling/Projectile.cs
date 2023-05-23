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
    public bool hit { get; set; }

    [SerializeField]
    private float damage;

    void Start()
    {
        projectileAnim = GetComponent<Animator>();
        spherecollider = GetComponent<SphereCollider>();
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
        if(hit)
            transform.position = Vector3.MoveTowards(transform.position, target, projectileSpeed * Time.deltaTime);

        if(!hit)
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
