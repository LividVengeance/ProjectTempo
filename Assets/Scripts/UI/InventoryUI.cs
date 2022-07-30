using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private GameObject ItemUIPrefab;
    [SerializeField] private Transform InventoryUIContent;
    [SerializeField] private Button InventoryCloseBttn;

    private InventoryComponent InventoryComponent;
    
    private bool bCanOpenInventory = true;
    private bool bCanCloseInventory = true;

    private void Start()
    {
        CloseInventory();
        InventoryComponent = TempoManager.Instance.GetHeroCharacter().GetHeroInventoryComponent(); 
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
        foreach (ItemBase Item in InventoryComponent.GetInventoryItemList())
        {
            var ItemUI = Instantiate(ItemUIPrefab, Vector3.zero, Quaternion.identity, InventoryUIContent);
            ItemUI.GetComponent<InventoryItemUI>().SetItemInfo(Item.ItemIcon, Item.ItemName, Item.ShortDescription,
                Item.LongDescription);
        }
    }
    
    public bool OpenInventory()
    {
        if (bCanOpenInventory)
        {
            transform.gameObject.SetActive(true);
            UpdateInventoryUI();
        }

        return bCanOpenInventory;
    }
    
    public bool CloseInventory()
    {
        if (bCanCloseInventory)
        {
            transform.gameObject.SetActive(false);
        }

        return bCanCloseInventory;
    }

    // Used for delegates, ignores callback
    private void CloseInventoryDelegate() => CloseInventory();
}
