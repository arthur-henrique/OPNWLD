using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : ObservableSubject
{
    [SerializeField] PlayerMovement player;
    private Transform playerOverWorldTransform;
    private Transform whereToSpawnAtOverworld;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void SetReturnCoordinates()
    {
        whereToSpawnAtOverworld = playerOverWorldTransform;
    }
    public void ReturnToOverworld()
    {
        player.transform.position = whereToSpawnAtOverworld.position;
    }
    private void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name != "1MainScene")
            player.gameObject.transform.position = Vector3.zero;
    }
}
