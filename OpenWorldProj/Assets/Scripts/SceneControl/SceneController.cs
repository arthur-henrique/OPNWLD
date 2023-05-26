using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;
    [SerializeField]
    private PlayerManager playerManager;
    [SerializeField]
    Transform zero;
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
    public void SetRespawnPoint(Transform newPoint)
    {
        playerManager.respawnPoint = newPoint;
    }
    IEnumerator SetPos()
    {
        yield return new WaitForSeconds(0.1f);
        print("Control");
        playerManager.SetTempleCoordinates();
        playerManager.respawnPoint = zero;
    }
}
