using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] string sceneToLoad;
    [SerializeField] Transform safePos;
    [SerializeField] GameManager gameManager;
    public bool isPortalToMainScene;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(isPortalToMainScene)
            {

                other.GetComponentInParent<PlayerManager>().SetReturnCoordinates(safePos);
            }
            gameManager.LevelTransfer(sceneToLoad);
        }
    }
}
