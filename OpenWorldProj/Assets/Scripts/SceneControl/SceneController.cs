using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private PlayerManager playerManager;
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        playerManager = gameManager.gameObject.GetComponent<PlayerManager>();
        StartCoroutine(SetPos());
    }

    

    public void OpenFirstSideDoor()
    {
        gameManager.GotSword();
        // Move the objects as to open a way into the dungeon;
    }
    IEnumerator SetPos()
    {
        yield return new WaitForSeconds(0.1f);
        print("Control");
        playerManager.SetTempleCoordinates();
        gameManager.EnableControl();
    }
}
