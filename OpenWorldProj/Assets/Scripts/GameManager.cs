using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] Animator transitionAnimator;
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
    
    public void LevelTransfer(string scene)
    {
        StartCoroutine(WaitToLoad(scene));
        transitionAnimator.SetTrigger("FADETOBLACK");
    }
    public void ClearUp()
    {
        transitionAnimator.SetTrigger("CLEARUP");
    }

    IEnumerator WaitToLoad(string scene)
    {
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(scene);
    }
    
}
