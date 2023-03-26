using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class HUDScreen : MenuScreen
{
    [Header("Health UI")]
    [SerializeField] private GameObject HealthUIGameobject;

    [Header("Experience UI")]
    [SerializeField] private GameObject ExperienceUIGameobject;
  

    public override bool OnActionDown(string InActionName)
    {
        bool bHandledInput = false;

        // Handle opening pause menu
        if (InActionName == "Pause" || InActionName == "Unpause")
        {
            MenuScreen PauseScreen;
            if (GetMenuManager().GetMenuLibrary().GetMenuScreens().TryGetValue(GetMenuManager().GetMenuLibrary().PauseScreenName, out PauseScreen))
            {
                PauseMenu PauseMenuScreen = (PauseMenu)PauseScreen;
                PauseMenuScreen.OpenPauseMenu();

                bHandledInput = true;
            }
        }

        // Handle opening inventory
        if (InActionName == "OpenCloseInventory")
        {
            MenuScreen InventoryScreen;
            if (GetMenuManager().GetMenuLibrary().GetMenuScreens().TryGetValue(GetMenuManager().GetMenuLibrary().InventoryScreenName, out InventoryScreen))
            {
                InventoryScreen InventoryMenuScreen = (InventoryScreen)InventoryScreen;
                InventoryMenuScreen.OpenInventory();

                bHandledInput = true;
            }
        }

        return bHandledInput;
    }
}