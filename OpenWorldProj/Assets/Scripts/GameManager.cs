using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour, IObserver
{
    [SerializeField] PlayerMovement player;
    private PlayerController playerC;

    public bool hasSword, hasSling;

    void Start()
    {
        playerC = player.inputActions;
        if(!hasSword)
        {
            playerC.Movement.Attack.Disable();
        }
        if(!hasSling)
        {
            playerC.Movement.Aim.Disable();
        }
    }

    public void GotSword()
    {
        playerC.Movement.Attack.Enable();
        // Enables the sword model
    }
    public void GotSling()
    {
        playerC.Movement.Aim.Enable();
        // Enables the sling model

    }
    public void OnNotifyDamage(float damage)
    {
        throw new System.NotImplementedException();
    }

    public void OnNotifyDeath()
    {
        throw new System.NotImplementedException();
    }
}
