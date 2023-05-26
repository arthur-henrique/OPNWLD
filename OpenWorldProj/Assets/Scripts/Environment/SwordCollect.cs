using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollect : MonoBehaviour
{
    [SerializeField] SceneController scene;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            scene.OpenFirstSideDoor();
            gameObject.SetActive(false);
        }
    }
}
