using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour, IPooledObject
{
    [SerializeField]
    private float projectileSpeed = 25f;
    private float timeToDeactivate = 5f;
    public Vector3 target { get;  set; }
    public bool hit { get; set; }
    public void OnObjectSpawn(Vector3 forward, bool hasHit)
    {
        //GetComponent<Rigidbody>().AddForce(forward * projectileSpeed, ForceMode.Impulse);
        target = forward;
        hit = hasHit;
        StartCoroutine(Deactivate());
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, projectileSpeed * Time.deltaTime);
        if(!hit && Vector3.Distance(transform.position, target) < 0.1f)
        {
            gameObject.SetActive(false);
        }
    }
    void OnCollisionEnter(Collision other)
    {
        //gameObject.SetActive(false);
    }

    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(timeToDeactivate );
        gameObject.SetActive(false);
    }
}
