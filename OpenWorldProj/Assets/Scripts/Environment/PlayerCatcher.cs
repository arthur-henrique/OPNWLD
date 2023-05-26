using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatcher : MonoBehaviour
{
    [SerializeField] private PlayerManager playerManager;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerManager = other.GetComponentInParent<PlayerManager>();
            playerManager.Respawn();
        }
         
    }
}
