using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    public GameObject imagemItem;
    public int contador, capacidadeMax;
    public TextMeshProUGUI contadorText;
    public GameObject inventarioCheio;
    public GameObject SemCura;



    public void PegouItem()
    {
        if(contador < capacidadeMax)
        {
            contador++;
            contadorText.text = "x" + contador.ToString();
            imagemItem.SetActive(true);

        }
        else
        {
            inventarioCheio.SetActive(true);
        }
    }

    public void UsouItem()
    {
        if(contador > 0)
        {
             contador--;
            contadorText.text = "x" + contador.ToString();
        }
        else
        {
            SemCura.SetActive(true);
        }



    }
}
