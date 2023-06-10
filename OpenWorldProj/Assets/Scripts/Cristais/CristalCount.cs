using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CristalCount : MonoBehaviour
{
    public int quantidadeDeCristais;
    public MovingPlatform movingPlatform;

    // Update is called once per frame
    void Update()
    {
        if (quantidadeDeCristais >= 3)
        {
            movingPlatform.enabled = true;
        }
        
    }
}
