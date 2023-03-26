using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class InputManager : MonoBehaviour
{
    public delegate void InputContextChanged(EInputType NewInputContext, FUserSettings.EInputIconType NewIconType);
    public InputContextChanged InputContextChangedDelegate;

    [Header("Virtual Cursor")]
    [SerializeField] private GameObject VirtualCursorGameObject;

    [Header("Settings")]
    [SerializeField] private bool bEnableDebugInfo = false;

    private PlayerInput PlayersInput;
    private InputMaster ActionInputMaster;
    private FGameMapInputs GameInputStruct = new FGameMapInputs();
    private Mouse CurrentMouse;
    private VirtualCursor VirtualCursor;
    private MenuManager MenuManager;
    private TempoGameUserSettings TempoGameUserSettings;    

    private HeroPlayerController HeroPlayerController;

    private bool bCursorEnabled = false;

    private EInputType CurrentInputType;
    private EInputType PreviousInputType;

    private const string GamepadControlScheme = "Gamepad";
    private const string MouseControlScheme = "Keyboard&Mouse";

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
        TempoGameUserSettings = TempoManager.Instance.GetGameUserSettings();

        PlayersInput = GetComponent<PlayerInput>();
        if (!PlayersInput) Debug.LogError("Unable to find valid PlayerInput");

        PlayersInput.onControlsChanged += OnInputContextChange;

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

        OnInputContextChange(PlayersInput);
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

    void OnInputContextChange(PlayerInput Input)
    {
        PreviousInputType = CurrentInputType;
        CurrentInputType = GetInputType(Input.currentControlScheme);

        if (InputContextChangedDelegate != null)
        {
            InputContextChangedDelegate.Invoke(CurrentInputType, GetIconType());
        }
    }

    private EInputType GetInputType(string InControlScheme)
    {
        if (InControlScheme.Equals(MouseControlScheme))
        {
            return EInputType.KeyboardMouse;
        }
        else if (InControlScheme.Equals(GamepadControlScheme))
        {
            return EInputType.Gamepad;
        }

        return EInputType.KeyboardMouse;
    }

    public FUserSettings.EInputIconType GetIconType()
    {
        switch(TempoGameUserSettings.GetInputIconType())
        {
            case FUserSettings.EInputIconType.Dynamic:
                {
                    switch (CurrentInputType)
                    {
                        case EInputType.KeyboardMouse: return FUserSettings.EInputIconType.KeyboardMouse;
                        case EInputType.Gamepad:
                            {
                                string CurrentGamepadName = PlayersInput.devices[0].name;
                                if (CurrentGamepadName.Contains("XInputControllerWindows"))
                                {
                                    return FUserSettings.EInputIconType.Xbox;
                                }
                                else if (CurrentGamepadName.Contains("DualSenseGamepadHID") || CurrentGamepadName.ToLower().Contains("playstation"))
                                {
                                    return FUserSettings.EInputIconType.Playstation;
                                }
                                else if (CurrentGamepadName.ToLower().Contains("switch"))
                                {
                                    return FUserSettings.EInputIconType.Switch;
                                }
                                return FUserSettings.EInputIconType.GamepadGeneric;
                            }
                        default:
                            return FUserSettings.EInputIconType.KeyboardMouse;
                    }
                }
            case FUserSettings.EInputIconType.KeyboardMouse: return FUserSettings.EInputIconType.KeyboardMouse;
            case FUserSettings.EInputIconType.Playstation: return FUserSettings.EInputIconType.Playstation;
            case FUserSettings.EInputIconType.Xbox: return FUserSettings.EInputIconType.Xbox;
            case FUserSettings.EInputIconType.Switch: return FUserSettings.EInputIconType.Switch;
            case FUserSettings.EInputIconType.GamepadGeneric: return FUserSettings.EInputIconType.GamepadGeneric;
            default: return FUserSettings.EInputIconType.KeyboardMouse;
        }
    }

    public EInputType GetCurrentInputType() => CurrentInputType;
    public EInputType GetPreviousInputType() => PreviousInputType;

    public bool IsMouseKeyboardInputType() => CurrentInputType == EInputType.KeyboardMouse;
    public bool IsGamepadInputType() => CurrentInputType == EInputType.Gamepad;

    public PlayerInput GetPlayerInput() => PlayersInput;
    public string GetCurrentControlScheme() => PlayersInput.currentControlScheme;

    public Vector2 GetMovementInputState() => GameInputStruct.Movement;

    // Cursor
    public bool IsCursorEnabled() => bCursorEnabled;
    public void SetCursorPosition(Vector2 InNewPosition) => CurrentMouse.WarpCursorPosition(InNewPosition);
    public VirtualCursor GetVirtualCursor() => VirtualCursor;
}