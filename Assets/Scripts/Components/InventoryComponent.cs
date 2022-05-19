using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryComponent : MonoBehaviour
{
    [Header("Inventory Settings")]
    [SerializeField] private int NumberInventorySlots;

    [Header("Inventory Items")]
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

    /// Removes given item from this inventory
    public void RemoveItemFromInventory(ItemBase ItemToRemove)
    {
        if (ItemContainer.Contains(ItemToRemove))
        {
            ItemContainer.Remove(ItemToRemove);
            BroadcastInventoryUpdate.Invoke();
        }
        else
        {
            Debug.LogWarning(transform.parent.gameObject.name + "'s Inventory Is Trying To Remove Item: " + ItemToRemove.ItemName
                + " That Is Not In This Inventory");
        }
    }

    public List<ItemBase> GetInventoryItemList() => ItemContainer;
    public UnityEvent GetInventoryUpdateEvent() => BroadcastInventoryUpdate;
}
