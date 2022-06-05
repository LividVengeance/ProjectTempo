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


    private void Start()
    {
        InputManager = TempoManager.Instance.GetInputManager();
    }

    private void Update()
    {
        if (InputManager.GetInventoryDownInputState())
        {
            // Close Inventory
            if (InventoryUIGameobject.activeSelf)
            {
                InventoryUIGameobject.GetComponent<InventoryUI>().CloseInventory();
                InventoryUIGameobject.SetActive(false);

                InputManager.DisableCursor();
            }
            // Open Inventory
            else
            {
                InventoryUIGameobject.GetComponent<InventoryUI>().OpenInventory();
                InventoryUIGameobject.SetActive(true);

                InputManager.EnableCursor();
            }
        }
    }

    public GameObject GetInventoryUIContent() => InventoryUIConentGameobject;
    public GameObject GetInventoryUIGameObject() => InventoryUIGameobject;
}
