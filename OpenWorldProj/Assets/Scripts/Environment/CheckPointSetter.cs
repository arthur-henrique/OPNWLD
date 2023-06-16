using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointSetter : MonoBehaviour
{
    [SerializeField] SceneController controller;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerManager.instance.respawnPoint = gameObject.transform;
            //controller.SetRespawnPoint(transform);
            gameObject.SetActive(false);
        }
    }
}
