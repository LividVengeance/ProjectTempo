using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    private InputManager InputManager;

    [Header("Health UI")]
    [SerializeField] private GameObject HealthUIGameobject;

    [Header("Experience UI")]
    [SerializeField] private GameObject ExperienceUIGameobject;
    
    [Header("Inventory UI")]
    [SerializeField] private GameObject InventoryUIGameobject;
    [SerializeField] private GameObject InventoryUIConentGameobject;

    [Header("Virtual Cursor")]
    [SerializeField] private VirtualCursor VirtualCursor;

    private CharacterController HeroCharacter;

    private void Start()
    {
        InputManager = TempoManager.Instance.GetInputManager();
        HeroCharacter = TempoManager.Instance.GetHeroCharacter();
    }

    private void Update()
    {
        if (InputManager.GetInventoryDownInputState())
        {
            // Close Inventory
            if (InventoryUIGameobject.activeSelf)
            {
                InventoryUIGameobject.GetComponent<InventoryUI>().CloseInventory();
                InputManager.DisableCursor();

                HeroCharacter.DisableHeroMovement(false);
            }
            // Open Inventory
            else
            {
                InventoryUIGameobject.GetComponent<InventoryUI>().OpenInventory();
                InputManager.EnableCursor();

                HeroCharacter.DisableHeroMovement(true);
            }
        }
    }

    public GameObject GetInventoryUIContent() => InventoryUIConentGameobject;
    public GameObject GetInventoryUIGameObject() => InventoryUIGameobject;
    public VirtualCursor GetVirtualCursor() => VirtualCursor;
}
