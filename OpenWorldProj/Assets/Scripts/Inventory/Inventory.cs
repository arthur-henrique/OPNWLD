using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private const int SLOTS = 1;
    private List<IInventoryItem> mItems = new List<IInventoryItem>();
    public event EventHandler<InventoryEventsArgs> ItemAdded;
    public event EventHandler<InventoryEventsArgs> ItemRemoved;
    public event EventHandler<InventoryEventsArgs> ItemUsed;
    public void AddItem (IInventoryItem item)
    {
        if(mItems.Count < SLOTS)
        {
            Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
            if (collider.enabled)
            {
                collider.enabled = false;
                mItems.Add(item);
                item.OnPickup();


                if(ItemAdded != null)
                {
                    ItemAdded(this, new InventoryEventsArgs(item));
                }
            }
        }
    }

    internal void UseItem(IInventoryItem item)
    {
        if (ItemUsed != null)
        {
            ItemUsed(this, new InventoryEventsArgs(item));
        }
    }

    public void RemoveItem(IInventoryItem item)
    {
        mItems.Remove(item);
        item.OnDrop();

        Collider collider = (item as MonoBehaviour).GetComponent<Collider>();
        if(collider != null)
        {
            collider.enabled = true;
        }
        if(ItemRemoved != null)
        {
            ItemRemoved(this, new InventoryEventsArgs(item));
        }
    }
}
