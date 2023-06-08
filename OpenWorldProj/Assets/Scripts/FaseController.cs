using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseController : MonoBehaviour
{
    public MovingPlatform platform;
    
    

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.childCount == 0)
        {
            platform.enabled = true;
        }
    }
}
