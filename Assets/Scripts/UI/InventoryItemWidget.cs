using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemWidget : MonoBehaviour
{
    [SerializeField] private Image ItemImageGameobject;
    [SerializeField] private TextMeshProUGUI ItemNameGameobject;

    public void SetItemInfo(Sprite InItemImage, string InItemName, string InItemShortDescription, string InItemLongDescription)
    {
        ItemImageGameobject.sprite = InItemImage;
        ItemNameGameobject.SetText(InItemName);
    }
}
