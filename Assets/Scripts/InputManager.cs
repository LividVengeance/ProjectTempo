using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class InputManager : MonoBehaviour
{
    [Header("Virtual Cursor")]
    [SerializeField] private GameObject VirtualCursorGameObject;

    [Header("Settings")]
    [SerializeField] private bool bEnableDebugInfo = false;

    private InputMaster ActionInputMaster;
    private FGameMapInputs GameInputStruct = new FGameMapInputs();
    private FMenuMapInput MenuInputStruct = new FMenuMapInput();
    private Mouse CurrentMouse;
    private VirtualCursor VirtualCursor;

    private bool bCursorEnabled = false;
    
    public struct FGameMapInputs
    {
        public Vector2 Movement;
        
        public bool InteractDown;
        
        public bool SaveDown;
        public bool LoadSaveDown;

        public bool ActionOneDown;
        public bool ActionTwoDown;
        public bool ActionThreeDown;

        public bool InventoryDown;
        public bool PauseDown;
    }

    public struct FMenuMapInput
    {
        public bool UnpauseDown;
        public bool CancelDown;
    };
    
    private void Awake()
    {
        ActionInputMaster = new InputMaster();
        CurrentMouse = Mouse.current;
        VirtualCursor = TempoManager.Instance.GetMenuManager().GetVirtualCursor();

        ActionInputMaster.Game.Movement.performed += ctx => GameInputStruct.Movement = ctx.ReadValue<Vector2>(); 
        ActionInputMaster.Game.Movement.canceled += ctx => GameInputStruct.Movement = ctx.ReadValue<Vector2>();
    }

    private void OnEnable() => ActionInputMaster.Enable();
    private void OnDisable() => ActionInputMaster.Disable();

    private void Update()
    {
        GameInputStruct.SaveDown = ActionInputMaster.Game.Save.triggered;
        GameInputStruct.LoadSaveDown = ActionInputMaster.Game.LoadSave.triggered;
        GameInputStruct.ActionOneDown = ActionInputMaster.Game.ActionOne.triggered;
        GameInputStruct.ActionTwoDown = ActionInputMaster.Game.ActionTwo.triggered;
        GameInputStruct.ActionThreeDown = ActionInputMaster.Game.ActionThree.triggered;
        GameInputStruct.InventoryDown = ActionInputMaster.Game.OpenCloseInventory.triggered;
        GameInputStruct.PauseDown = ActionInputMaster.Game.Pause.triggered;

        MenuInputStruct.UnpauseDown = ActionInputMaster.Menu.Unpause.triggered;
        MenuInputStruct.CancelDown = ActionInputMaster.Menu.Cancel.triggered;
    }

    public void EnableCursor()
    {
        bCursorEnabled = true;
        if (!VirtualCursorGameObject)
        {
            Debug.LogError("Unable to get virtual cursor gameobject reference");
        }
        else
        {
            VirtualCursor.EnableCursor();
            VirtualCursorGameObject.SetActive(true);
        }
        Cursor.lockState = CursorLockMode.None;
        SetCursorCentre();
    }

    public void DisableCursor()
    {
        bCursorEnabled = false;
        if (!VirtualCursorGameObject)
        {
            Debug.LogError("Unable to get virtual cursor gameobject reference");
        }
        else
        {
            VirtualCursorGameObject.SetActive(false);
        }
        Cursor.lockState = CursorLockMode.None;
    }

    public void SetCursorCentre()
    {
        Vector2 ScreenCentrePosition = new Vector2(Screen.height / 2, Screen.width / 2);
        CurrentMouse.WarpCursorPosition(ScreenCentrePosition);
        VirtualCursorGameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(ScreenCentrePosition.x, ScreenCentrePosition.y, 100.0f);
    }

    public void SwitchToGameMap()
    {
        ActionInputMaster.Game.Enable();
        ActionInputMaster.Menu.Disable();
        if (bEnableDebugInfo)
        {
            Debug.Log("Switch To Game Input Mapping");
        }
    }

    public void SwitchToMenuMap()
    {
        ActionInputMaster.Game.Disable();
        ActionInputMaster.Menu.Enable();
        if (bEnableDebugInfo)
        {
            Debug.Log("Switch To Menu Input Mapping");
        }
    }

    // Game Map
    public Vector2 GetMovementInputState() => GameInputStruct.Movement;

    public bool GetInteractDownInputState() => GameInputStruct.InteractDown;

    public bool GetSaveDownInputState() => GameInputStruct.SaveDown;
    public bool GetLoadSaveDownState() => GameInputStruct.LoadSaveDown;

    public bool GetActionOneDownInputState() => GameInputStruct.ActionOneDown;
    public bool GetActionTwoDownInputState() => GameInputStruct.ActionTwoDown;
    public bool GetActionThreeDownInputState() => GameInputStruct.ActionThreeDown;

    public bool GetInventoryDownInputState() => GameInputStruct.InventoryDown;
    public bool GetGamePauseDownInputState() => GameInputStruct.PauseDown;

    // Menu Map
    public bool GetMenuCancelDownState() => MenuInputStruct.CancelDown;
    public bool GetMenuUnpauseDownInputState() => MenuInputStruct.UnpauseDown;

    // Maps
    public void EnableGameInputs() => ActionInputMaster.Game.Enable();
    public void DisableGameInputs() => ActionInputMaster.Game.Disable();

    public void EnableMenuInputs() => ActionInputMaster.Menu.Enable();
    public void DisableMenuInputs() => ActionInputMaster.Menu.Disable();

    // Cursor
    public bool IsCursorEnabled() => bCursorEnabled;
    public void SetCursorPosition(Vector2 InNewPosition) => CurrentMouse.WarpCursorPosition(InNewPosition);
    public VirtualCursor GetVirtualCursor() => VirtualCursor;
}