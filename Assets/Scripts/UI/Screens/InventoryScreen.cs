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

    private void Start()
    {
        MenuManager = TempoManager.Instance.GetMenuManager();
        InventoryComponent = TempoManager.Instance.GetHeroCharacter().GetHeroInventoryComponent();
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

    public override void SwitchTo()
    {
        base.SwitchTo();
        UpdateInventoryUI();
    }

    public override bool OnActionDown(string InActionName)
    {
        if (!base.OnActionDown(InActionName))
        {
            if (InActionName == "OpenCloseInventory")
            {
                CloseInventory();
                // Consume Input
                return true;
            }
        }
        return base.OnActionDown(InActionName);
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
    
    public void CloseInventory()
    {
        MenuManager.StartTransitionToHUDScreen(DefaultTransitionSettings);
    }

    // Used for delegates, ignores callback
    private void CloseInventoryDelegate() => CloseInventory();
}
