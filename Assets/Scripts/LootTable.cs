using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class LootTable : MonoBehaviour
{
    [Serializable]
    public class FLootTable
    {
        public ItemBase Item; // Item class to drop
        [Range(1, 100)] public int ItemWeight; // Chance item can drop
        [MinMaxSlider(1, 999)] public Vector2Int StackSize; // The amount of this item that can drop
    }

    [TableList(AlwaysExpanded = true, DrawScrollView = false)]
    public List<FLootTable> LocalLootTable = new List<FLootTable>() { new FLootTable() };

    private List<ItemBase> ItemsToDrop = new List<ItemBase>();

    private void Start()
    {
        CalculateItemsToDrop();
    }

    /// Determines what items are going to drop
    private void CalculateItemsToDrop()
    {
        // Ensure there are no items in the list
        ItemsToDrop.Clear();

        foreach (FLootTable CurrentItem in LocalLootTable)
        {
            int ItemDropWeight = UnityEngine.Random.Range(1, 100);
            // Chance the item will drop
            if (ItemDropWeight <= CurrentItem.ItemWeight)
            {
                int NumberOfItemsToDrop = UnityEngine.Random.Range(CurrentItem.StackSize.x, CurrentItem.StackSize.y);

                // Adds the current item for as many times as the stack size
                for (int CurrentIndex = 0; CurrentIndex <= NumberOfItemsToDrop; CurrentIndex++)
                {
                    ItemsToDrop.Add(CurrentItem.Item);
                }
            }
        }
    }

    /// Modifies the stack size for a given item
    public void ModifyStackSizeForItem(ItemBase InItem, int NewStackSize, bool SetMin)
    {
        foreach (FLootTable CurrentLootItem in LocalLootTable)
        {
            // Check the InItem is the current item
            if (CurrentLootItem.Item.GetType() == InItem.GetType())
            {
                if (SetMin) CurrentLootItem.StackSize.x = NewStackSize;
                else CurrentLootItem.StackSize.y = NewStackSize;
                // Recalculate the items to drop using new stack size
                CalculateItemsToDrop();
                return;
            }
        }

        Debug.LogWarning(transform.parent.gameObject.name + "'s LootTable Does Not Contain Item: " + InItem.name);
    }

    /// Modifies the stack weight of given item
    public void ModifyItemWeightForItem(ItemBase InItem, int NewStackWeight)
    {
        foreach (FLootTable CurrentLootItem in LocalLootTable)
        { 
            // Check the InItem is the current item
            if (CurrentLootItem.Item.GetType() == InItem.GetType())
            { 
                CurrentLootItem.ItemWeight = NewStackWeight;
                // Recalculates the items to drop using new stack weight
                CalculateItemsToDrop();
                return;
            }
        }

        Debug.LogWarning(transform.parent.gameObject.name + "'s LootTable Does Not Contain Item: " + InItem.name);
    }

    /// Gets the items this loot table will drop
    public List<ItemBase> GetDropItems() => ItemsToDrop;

    /// Gets this loot table
    public List<FLootTable> GetLootTable() => LocalLootTable;

    /// Will recalculate the items to drop
    public void ReroleItemsToDrop() => CalculateItemsToDrop();
}