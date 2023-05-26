using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour, IObserver
{
    [SerializeField] PlayerMovement player;
    private PlayerController playerC;

    public bool hasSword, hasSling;
    public GameObject swordModel, sideSlingModel;

    void Start()
    {
        playerC = player.inputActions;
        if(!hasSword)
        {
            playerC.Movement.Attack.Disable();
            swordModel.SetActive(false);
        }
        if(!hasSling)
        {
            playerC.Movement.Aim.Disable();
            sideSlingModel.SetActive(false);
        }
    }

    public void GotSword()
    {
        hasSword = true;
        playerC.Movement.Attack.Enable();
        swordModel.SetActive(true);
    }
    public void GotSling()
    {
        hasSling = true;
        playerC.Movement.Aim.Enable();
        sideSlingModel.SetActive(true);

    }
    public void DisableControl()
    {
        playerC.Disable();
    }
    public void EnableControl()
    {
        playerC.Enable();
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
