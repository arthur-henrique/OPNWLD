using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CristalDetect : MonoBehaviour
{
    Animator anim;
    public CristalCount cristalCount;
    

    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerProjectile"))
        {
            cristalCount.quantidadeDeCristais++;
            anim.SetBool("Ativado", true);
            gameObject.GetComponent<Collider>().enabled = false;

        }
    }


  
}
