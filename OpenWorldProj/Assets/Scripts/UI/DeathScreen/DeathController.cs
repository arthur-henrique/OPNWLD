using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathController : MonoBehaviour
{
    public GameObject canvasPlayer, player;
    
    private void Update()
    {
        canvasPlayer.SetActive(false);
        player.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        PlayerManager.instance.player.DisableController();
        //Time.timeScale = 0;
    }
    public void Respawn()
    {
        Cursor.lockState = CursorLockMode.Locked;
        canvasPlayer.SetActive(true);
        player.SetActive(true);
        gameObject.SetActive(false);
        Time.timeScale = 1;
        PlayerManager.instance.Respawn();


    }
        

    public void CarregarMainMenu()
    {
        //SceneManager.LoadScene("MainMenu");
        gameObject.SetActive(false);
        Time.timeScale = 1;
        PlayerManager.instance.player.EnableController();
    }
}
