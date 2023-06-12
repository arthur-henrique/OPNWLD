using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : ObservableSubject
{
    public static PlayerManager instance;
    [SerializeField] public PlayerMovement player;
    [SerializeField] private Transform whereToSpawnAtOverworld;
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject playerObj;
    [SerializeField] public HealthControl playerHealthControl;
    public HealthSliderControl sliderControl;
    private Transform playerOverWorldTransform;
    public Vector3 pos;

    public Transform respawnPoint;
    public float valueToHeal;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        pos = transform.position;
        instance = this;
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

    public void Respawn()
    {
        player.DisableController();
        playerObj.transform.position = respawnPoint.position;
        player.EnableController();

    }

    public void RecoverHealth()
    {
        playerHealthControl.health += valueToHeal;
        sliderControl.OnNotifyHeal(valueToHeal); 
    }
    private void OnLevelWasLoaded(int level)
    {
        gameManager.ClearUp();
        if (SceneManager.GetActiveScene().name == "1MainScene")
        {
            ReturnToOverworld();
        }
    }
}
