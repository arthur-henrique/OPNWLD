using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotion : MonoBehaviour, IInventoryItem
{
    public string Name
    {
        get
        {
            return "HealthPotion";
        }
    }
    public Sprite _image = null;
    public Sprite Image
    {
        get
        {
            return _image;
        }
    }
   
    public void OnPickup()
    {
        gameObject.SetActive(false);
    }

    public void OnDrop()
    {
        throw new System.NotImplementedException();
    }

    public void OnUse()
    {
        Destroy(this.gameObject, 2f);
    }
}
