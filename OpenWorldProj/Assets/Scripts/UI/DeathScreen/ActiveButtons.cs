using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveButtons : MonoBehaviour
{
    public GameObject button1, button2;

    public void AtivaBotao()
    {
        button1.SetActive(true);
        button2.SetActive(true);
    }
}
