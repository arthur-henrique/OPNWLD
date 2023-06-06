using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
public interface IInventoryItem
{
    string Name { get; }
    Sprite Image { get; }
    void OnPickup();
    void OnDrop();
    void OnUse();
    
}
public class InventoryEventsArgs : EventArgs
{
    public InventoryEventsArgs(IInventoryItem item)
    {
        Item = item;
    }
    public IInventoryItem Item;

}

public class InventoryItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
