using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
