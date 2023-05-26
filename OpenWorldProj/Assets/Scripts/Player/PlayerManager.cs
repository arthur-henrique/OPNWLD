using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : ObservableSubject
{
    [SerializeField] PlayerMovement player;
    [SerializeField] private Transform whereToSpawnAtOverworld;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject playerObj;
    private Transform playerOverWorldTransform;
    public Vector3 pos;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        pos = transform.position;
    }
    
    public void SetReturnCoordinates(Transform safePos)
    {
        whereToSpawnAtOverworld = safePos;
        pos = whereToSpawnAtOverworld.position;
    }
    public void SetTempleCoordinates()
    {
        transform.position = Vector3.zero;
        player.DisableController();
        playerObj.transform.position = transform.position;
        player.EnableController();
    }
    public void ReturnToOverworld()
    {
        transform.position = pos;
        player.DisableController();
        playerObj.transform.position = transform.position;
        player.EnableController();

    }

    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name == "1MainScene")
        {
            ReturnToOverworld();
        }
    }
}
