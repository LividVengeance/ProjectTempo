using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class InputManager : MonoBehaviour
{
    [SerializeField] private bool bDisableCursorOnPlay = false;

    private InputMaster ActionInputMaster;
    private PlayerCharacterInputs InputStruct = new PlayerCharacterInputs();
    
    public struct PlayerCharacterInputs
    {
        public Vector2 Movement;
        
        public bool InteractDown;
        
        public bool SaveDown;
        public bool LoadSaveDown;

        public bool ActionOneDown;
        
        public bool ActionTwoDown;
        
        public bool ActionThreeDown;

        public bool InventoryDown;
    }
    
    private void Awake()
    {
        ActionInputMaster = new InputMaster();

        ActionInputMaster.Game.Movement.performed += ctx => InputStruct.Movement = ctx.ReadValue<Vector2>(); 
        ActionInputMaster.Game.Movement.canceled += ctx => InputStruct.Movement = ctx.ReadValue<Vector2>();

        if (bDisableCursorOnPlay)
        {
            DisableCursor();
        }
    }

    private void OnEnable() => ActionInputMaster.Enable();
    private void OnDisable() => ActionInputMaster.Disable();

    private void Update()
    {
        //TODO: Find away to have these inputs not handled each tick
        InputStruct.SaveDown = ActionInputMaster.Game.Save.triggered;
        InputStruct.LoadSaveDown = ActionInputMaster.Game.LoadSave.triggered;
        InputStruct.ActionOneDown = ActionInputMaster.Game.ActionOne.triggered;
        InputStruct.ActionTwoDown = ActionInputMaster.Game.ActionTwo.triggered;
        InputStruct.ActionThreeDown = ActionInputMaster.Game.ActionThree.triggered;
        InputStruct.InventoryDown = ActionInputMaster.Game.OpenCloseInventory.triggered;
    }

    public void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;
    }

    public Vector2 GetMovementInputState() => InputStruct.Movement;

    public bool GetInteractDownInputState() => InputStruct.InteractDown;

    public bool GetSaveDownInputState() => InputStruct.SaveDown;
    public bool GetLoadSaveDownState() => InputStruct.LoadSaveDown;

    public bool GetActionOneDownInputState() => InputStruct.ActionOneDown;
    public bool GetActionTwoDownInputState() => InputStruct.ActionTwoDown;
    public bool GetActionThreeDownInputState() => InputStruct.ActionThreeDown;

    public bool GetInventoryDownInputState() => InputStruct.InventoryDown;

    public void EnableGameInputs() => ActionInputMaster.Game.Enable();
    public void DisableGameInputs() => ActionInputMaster.Game.Disable();

    public void EnableMenuInputs() => ActionInputMaster.Menu.Enable();
    public void DisableMenuInputs() => ActionInputMaster.Menu.Disable();
}