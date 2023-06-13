using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseController : MonoBehaviour
{
    public MovingPlatform platform;
    public MovingPlatafomrPlayer platafomrPlayer;
    public Animator ponte;
    
    

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.childCount == 0)
        {
            if(platafomrPlayer != null)
            {
                platafomrPlayer.enabled = true;
            }
            if(platform != null)
            {

              platform.enabled = true;
            }

            if(ponte != null)
            {
                ponte.SetBool("isMoving", true);
            }
        }
    }
}
