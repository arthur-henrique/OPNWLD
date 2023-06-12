using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CristalMovePedra : MonoBehaviour
{
    Animator anim;
    public Animator pedraAnim;
    [SerializeField] bool cristalAtivado;
   
    private void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerArrow"))
        {
            
            anim.SetBool("Ativado", true);
            pedraAnim.SetBool("IsMoving", true);
               
               
            

            

        }
    }

  
}
