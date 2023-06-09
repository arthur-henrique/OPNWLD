using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCanvas : MonoBehaviour
{
    public GameObject canvas;
    Collider colliderObj;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            colliderObj = gameObject.GetComponent<Collider>();
            colliderObj.enabled = false;
            canvas.SetActive(true);
            
        }
    }
}
