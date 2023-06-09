using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCanvas : MonoBehaviour
{
    public GameObject canvas;
    Collider collider;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            collider = gameObject.GetComponent<Collider>();
            collider.enabled = false;
            canvas.SetActive(true);
            
        }
    }
}
