using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] string sceneToLoad;
    [SerializeField] Transform safePos;
    public bool isPortalToMainScene;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMovement>().DisableController();
            if(isPortalToMainScene)
            {
                other.GetComponentInParent<GameManager>().DisableControl();

                other.GetComponentInParent<PlayerManager>().SetReturnCoordinates(safePos);
                print("HasSet");
            }
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
