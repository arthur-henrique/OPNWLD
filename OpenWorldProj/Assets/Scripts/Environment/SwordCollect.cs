using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollect : MonoBehaviour
{
    [SerializeField] SceneController scene;
    public GameObject enemys;
    public bool isSling;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(!isSling)
            {
                scene.OpenFirstSideDoor();
                gameObject.SetActive(false);
                if(enemys != null)
                enemys.SetActive(true);
            }
            else
            {
                scene.OpenSecondSideDoor();
                gameObject.SetActive(false);
                
            }
            
        }
    }
}
