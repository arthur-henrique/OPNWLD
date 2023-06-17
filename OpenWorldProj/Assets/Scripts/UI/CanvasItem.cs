using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class CanvasItem : MonoBehaviour
{
    public GameObject parentPotion;
    public GameObject enemySpawn;
    public Animator anim;
    public KeyControl escapeKey { get; }
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.None;
        PlayerManager.instance.player.DisableController();
    }
    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            PlayerManager.instance.player.EnableController();
            Cursor.lockState = CursorLockMode.Locked;
            anim.SetBool("Saindo", true);
          
        }
    }

    public void ReligarCursor()
    {
        
       PlayerManager.instance.player.EnableController();
       Cursor.lockState = CursorLockMode.Locked;
        anim.SetBool("Saindo", true);

    }
    public void DestruirCanvas()
    {
        if (enemySpawn != null)
        {
            enemySpawn.SetActive(true);

        }
        Destroy(parentPotion);
    }

    
}
