using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemClickerHandler : MonoBehaviour
{
    public Inventory _inventory;
    public ItemDragHandler dragHandler;
   public void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
    
            IInventoryItem item = dragHandler.Item;

             Debug.Log(item.Name);
            _inventory.UseItem(item);
             item.OnUse();

        }
    }

}
