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
    private Mouse CurrentMouse;
    private VirtualCursor VirtualCursor;
    private MenuManager MenuManager;

    private HeroPlayerController HeroPlayerController;

    private bool bCursorEnabled = false;

    public enum EInputType
    {
        KeyboardMouse,
        Gamepad,
    }

    public struct FGameMapInputs
    {
        public Vector2 Movement;
    }

    private void Awake()
    {
        ActionInputMaster = new InputMaster();
        CurrentMouse = Mouse.current;
        VirtualCursor = TempoManager.Instance.GetMenuManager().GetVirtualCursor();
        HeroPlayerController = TempoManager.Instance.GetHeroCharacter().GetHeroController();
        MenuManager = TempoManager.Instance.GetMenuManager();

        SwitchToGameMap();

        // Setup input delegates
        ActionInputMaster.Game.Movement.performed += ctx => GameInputStruct.Movement = ctx.ReadValue<Vector2>();
        ActionInputMaster.Game.Movement.canceled += ctx => GameInputStruct.Movement = ctx.ReadValue<Vector2>();
        ActionInputMaster.Game.Movement.performed += OnInputActionDown;
        ActionInputMaster.Game.Movement.canceled += OnInputActionDown;

        // Save load
        ActionInputMaster.Game.Save.started += OnInputActionDown;
        ActionInputMaster.Game.Save.canceled += OnInputActionDown;
        ActionInputMaster.Game.LoadSave.started += OnInputActionDown;
        ActionInputMaster.Game.LoadSave.canceled += OnInputActionDown;

        // Action keys
        ActionInputMaster.Game.ActionOne.started += OnInputActionDown;
        ActionInputMaster.Game.ActionOne.canceled += OnInputActionDown;
        ActionInputMaster.Game.ActionTwo.started += OnInputActionDown;
        ActionInputMaster.Game.ActionTwo.canceled += OnInputActionDown;
        ActionInputMaster.Game.ActionThree.started += OnInputActionDown;
        ActionInputMaster.Game.ActionThree.canceled += OnInputActionDown;

        ActionInputMaster.Game.OpenCloseInventory.started += OnInputActionDown;
        ActionInputMaster.Game.OpenCloseInventory.canceled += OnInputActionDown;
        ActionInputMaster.Game.Pause.started += OnInputActionDown;
        ActionInputMaster.Game.Pause.canceled += OnInputActionDown;

        ActionInputMaster.Menu.Unpause.started += OnInputActionDown;
        ActionInputMaster.Menu.Unpause.canceled += OnInputActionDown;
        ActionInputMaster.Menu.Cancel.started += OnInputActionDown;
        ActionInputMaster.Menu.Cancel.canceled += OnInputActionDown;
    }

    //TODO: This delegate is not being fired on input action performed
    public void OnInputActionDown(InputAction.CallbackContext InContext)
    {
        bool bHandledInput = false;
        // Slate input handling
        if (MenuManager)
        {
            switch (InContext.phase)
            {
                case InputActionPhase.Started:
                {
                    bHandledInput = MenuManager.OnActionDown(InContext.action.name);
                    break;
                }
                case InputActionPhase.Canceled:
                {
                    bHandledInput = MenuManager.OnActionUp(InContext.action.name);
                    break;
                }
                default:
                {
                    break;
                }
            }
        }

        if (bHandledInput)
        {
            return;
        }

        if (HeroPlayerController)
        {
            HeroPlayerController.OnInputActionRecieved(InContext);
        }
    }

    private void OnEnable() => ActionInputMaster.Enable();
    private void OnDisable() => ActionInputMaster.Disable();

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

    public Vector2 GetMovementInputState() => GameInputStruct.Movement;

    // Cursor
    public bool IsCursorEnabled() => bCursorEnabled;
    public void SetCursorPosition(Vector2 InNewPosition) => CurrentMouse.WarpCursorPosition(InNewPosition);
    public VirtualCursor GetVirtualCursor() => VirtualCursor;
}