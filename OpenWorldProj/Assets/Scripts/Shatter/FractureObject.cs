using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureObject : MonoBehaviour
{
    public GameObject originalObject;
    public GameObject fractureObject;
    public GameObject explosinVFX;
    public float explosionMinForce = 5;
    public float explosionMaxForce = 100;
    public float explosionForceRadius = 10;
    public float fragScaleFactor = 1;
    public Animator animator;
    public CapsuleCollider capsule;

    private GameObject fractObj;
    
    public void Explode()
    {
        if(originalObject != null)
        {
            originalObject.SetActive(false);
            if(fractureObject != null)
            {
                capsule.enabled = true;
                fractObj = Instantiate(fractureObject,originalObject.transform.position, originalObject.transform.rotation);
              
                foreach (Transform t in fractObj.transform)
                {
                    var rb = t.GetComponent<Rigidbody>();
                    if(rb != null)
                    {
                        rb.AddExplosionForce(Random.Range(explosionMinForce, explosionMaxForce), originalObject.transform.position, explosionForceRadius);
                    }
                    StartCoroutine(Shirink(t, 2));
                }
               
                Destroy(fractObj, 5);

                if(explosinVFX != null)
                {
                    GameObject exploVFX = Instantiate(explosinVFX,originalObject.transform.position, originalObject.transform.rotation);
                    Destroy(exploVFX, 7);
                }
                animator.enabled = false;
                capsule.enabled = false;
             
            }
        }
    }
    IEnumerator Shirink(Transform t, float delay)
    {
        yield return new WaitForSeconds(delay);
        Vector3 newScale = t.localScale;
        while(newScale.x >= 0)
        {
            newScale -= new Vector3(fragScaleFactor, fragScaleFactor, fragScaleFactor);
            t.localScale = newScale;
            Destroy(gameObject);
            yield return new WaitForSeconds(0.05f);
        }
    }

    
}
