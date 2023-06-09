using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasItem : MonoBehaviour
{
    public GameObject parentPotion;
    void Update()
    {
        Cursor.lockState = CursorLockMode.None;
        PlayerManager.instance.player.DisableController();
    }
   
    public void ReligarCursor()
    {
        PlayerManager.instance.player.EnableController();
        Cursor.lockState = CursorLockMode.Locked;
        Destroy(parentPotion);
    }

    
}
