using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jogar : MonoBehaviour
{
    public GameObject credits;
    public GameObject credits2;
    // Start is called before the first frame update
    public void Play()
    {

        SceneManager.LoadScene("0Void");
    }

    


    public void Quit()
    {

        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            credits.SetActive(false);
            credits2.SetActive(true);
        }

    }
}
