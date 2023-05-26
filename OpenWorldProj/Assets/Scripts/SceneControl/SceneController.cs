using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    private GameManager gameManager;
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    

    public void OpenFirstSideDoor()
    {
        gameManager.GotSword();
        // Move the objects as to open a way into the dungeon;
    }
}
