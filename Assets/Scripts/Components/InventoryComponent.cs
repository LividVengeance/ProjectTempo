using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryComponent : MonoBehaviour
{
    [SerializeField] private List<ItemBase> ItemContainer = new List<ItemBase>();
    private UnityEvent BroadcastInventoryUpdate;

    private void Awake()
    {
        BroadcastInventoryUpdate = new UnityEvent();
    }

    public void AddItemToInventory(ItemBase ItemToAdd)
    {
        ItemContainer.Add(ItemToAdd);
        BroadcastInventoryUpdate.Invoke();
    }

    public void RemoveItemFromInventory(ItemBase ItemToRemove)
    {
        ItemContainer.Remove(ItemToRemove);
        BroadcastInventoryUpdate.Invoke();
    }

    public List<ItemBase> GetInventoryItemList() => ItemContainer;
    public UnityEvent GetInventoryUpdateEvent() => BroadcastInventoryUpdate;
}
