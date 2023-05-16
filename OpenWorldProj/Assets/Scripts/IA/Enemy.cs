using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    public Animator animator;
   

    public void Die()
    {

        GetComponent<Collider>().enabled = false;
        

    }
  
}
