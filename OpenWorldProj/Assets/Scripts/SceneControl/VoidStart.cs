using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VoidStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        SceneManager.LoadScene("1MainScene");
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.1f);
    }
}
