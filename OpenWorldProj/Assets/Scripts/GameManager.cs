using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [SerializeField] Animator transitionAnimator;
    [SerializeField] PlayerMovement player;
    [SerializeField] HealthControl playerHealth;
    private PlayerController playerC;
    private PlayerController managerActions;


    public bool hasSword, hasSling;
    public GameObject swordModel, sideSlingModel;


    private void Awake()
    {
        managerActions = new PlayerController();

    }

    private void OnEnable()
    {
        managerActions.Enable();
    }
    private void OnDisable()
    {
        managerActions.Disable();
    }
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


    private void Update()
    {
        if(managerActions.CheatCode.Invincible.WasPressedThisFrame())
        {
            playerHealth.health = 150000f;
        }

        if(managerActions.CheatCode.Vincible.WasPressedThisFrame())
        {
            playerHealth.health = 100f;
        }

        if(managerActions.CheatCode.MoveToSafePos.WasPressedThisFrame())
        {
            LevelTransfer("1MainScene");
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
        StartCoroutine(WaitToDisable());
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

    IEnumerator WaitToDisable()
    {
        yield return new WaitForSeconds(0.15f);
        player.DisableController();
    }
    
}
