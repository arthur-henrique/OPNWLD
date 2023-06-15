using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonMaterial : MonoBehaviour
{
    public Material MaterialDoDragao;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
       StartCoroutine(ChangeColorDelay());
    }

    /// <summary>
    /// Changes the Dragon's material color for damage
    /// </summary>
    /// <returns></returns>
    IEnumerator ChangeColorDelay()
    {
       for (int i = 0; i < 11; i++)
       {

           print(i);
           MaterialDoDragao.SetFloat("_TurnRed", i / 10f);
           yield return new WaitForSecondsRealtime(0.014f);
       }
        yield return new WaitForSecondsRealtime(0.08f);
        for (int i = 9; i > -1; i--)
        {
            print(i);
            MaterialDoDragao.SetFloat("_TurnRed", i / 10f);
            yield return new WaitForSecondsRealtime(0.014f);
        }
    }

 }

//if (MaterialDoDragao.GetFloat("_TurnRed") == 0)
//MaterialDoDragao.SetFloat("_TurnRed", 0);
