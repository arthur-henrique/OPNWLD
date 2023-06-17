using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollect : MonoBehaviour
{
    [SerializeField] SceneController scene;
    public bool isSling;
    public GameObject canvasTutorial;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(!isSling)
            {
                scene.OpenFirstSideDoor();
                canvasTutorial.SetActive(true);
                
               

            }
            else
            {
                scene.OpenSecondSideDoor();
                canvasTutorial.SetActive(true);
                

            }
            
        }
    }
}
