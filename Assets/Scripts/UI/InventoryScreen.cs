using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScreen : MenuScreen
{
    [SerializeField] private GameObject ItemUIPrefab;
    [SerializeField] private Transform InventoryUIContent;
    [SerializeField] private Button InventoryCloseBttn;

    private InventoryComponent InventoryComponent;
    private MenuManager MenuManager;
    
    private bool bCanOpenInventory = true;
    private bool bCanCloseInventory = true;

    private void Start()
    {
        MenuManager = TempoManager.Instance.GetMenuManager();
        
        CloseInventory();
        InventoryComponent.GetInventoryUpdateEvent().AddListener(UpdateInventoryUI);
    }

    private void OnEnable()
    {
        InventoryCloseBttn.onClick.AddListener(CloseInventoryDelegate);
    }

    private void OnDisable()
    {
        InventoryCloseBttn.onClick.RemoveAllListeners();
    }


    private void UpdateInventoryUI()
    {
        // Destroy Inventory Item UI
        foreach (Transform Child in InventoryUIContent)
        {
            GameObject.Destroy(Child.gameObject);
        }
        
        // Populate Inventory UI
        foreach (ItemBase Item in TempoManager.Instance.GetHeroCharacter().GetHeroInventoryComponent().GetInventoryItemList())
        {
            var ItemUI = Instantiate(ItemUIPrefab, Vector3.zero, Quaternion.identity, InventoryUIContent);
            ItemUI.GetComponent<InventoryItemWidget>().SetItemInfo(Item.ItemIcon, Item.ItemName, Item.ShortDescription,
                Item.LongDescription);
        }
    }
    
    public bool OpenInventory()
    {
        if (bCanOpenInventory)
        {
            TempoManager.Instance.GetMenuManager().StartTransitionToScreen(DefaultTransitionSettings);
            UpdateInventoryUI();
        }

        return bCanOpenInventory;
    }
    
    public bool CloseInventory()
    {
        if (bCanCloseInventory)
        {
            MenuManager.StartTransitionToScreen(DefaultTransitionSettings);
        }

        return bCanCloseInventory;
    }

    // Used for delegates, ignores callback
    private void CloseInventoryDelegate() => CloseInventory();
}
