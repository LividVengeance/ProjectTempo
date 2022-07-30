using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class VirtualCursor : MonoBehaviour
{
    [SerializeField] private PlayerInput PlayerInput;
    [SerializeField] private RectTransform CursorInstanceTransfrom;
    [SerializeField] private float DefaultVirtualCursorSpeed = 1000;
    [SerializeField] private RectTransform CanvasTransform;
    [SerializeField] private Camera GameCamera;
    [SerializeField] private Canvas Canvas;
    
    private float VirtualCursorSpeed;
    private bool bPreviousCursorState;

    private Mouse VirtualCursorUI = null;
    private Mouse CurrentMouse;
    private InputManager InputManager;

    [Header("Genral Cursor Settings")]
    [SerializeField] private bool bEnableCursorOnPlay = false;
    [SerializeField] private bool bEnaleDebugInfo = false;

    [Header("Virtual Cursor")]
    [SerializeField] private float VerticalPadding = 35.0f;
    [SerializeField] private float HorizontalPadding = 35.0f;

    private string PerviousControlScheme = "";
    private const string GamepadControlScheme = "Gamepad";
    private const string MouseControlScheme = "Keyboard&Mouse";

    private void Start()
    {
        if (bEnableCursorOnPlay)
        {
            EnableCursor();
        }
        else
        {
            DisableCursor();
        }
    }

    private void OnEnable()
    {
        GameCamera = Camera.main;
        CurrentMouse = Mouse.current;
        InputManager = TempoManager.Instance.GetInputManager();
        VirtualCursorSpeed = DefaultVirtualCursorSpeed;

        if (VirtualCursorUI == null)
        {
            VirtualCursorUI = (Mouse)InputSystem.AddDevice("VirtualMouse");
        }
        else if (!VirtualCursorUI.added)
        {
            InputSystem.AddDevice(VirtualCursorUI);
        }

        // Pair the device to the user to use the PlayerInput compoent with the event system & the virtual mouse
        InputUser.PerformPairingWithDevice(VirtualCursorUI, PlayerInput.user);

        if (VirtualCursorUI != null)
        {
            Vector2 Postion = CursorInstanceTransfrom.anchoredPosition;
            InputState.Change(VirtualCursorUI.position, Postion);
        }

        // Update the current control context before an input is switched
        if (PlayerInput.currentControlScheme == MouseControlScheme)
        {
            SwitchToMouseCursor();
        }
        else if (PlayerInput.currentControlScheme == GamepadControlScheme)
        {
            SwitchToVirtualCursor();
        }

        InputSystem.onAfterUpdate += UpdateMotion;
        PlayerInput.onControlsChanged += UpdateControlContext;
    }

    private void OnDisable()
    {
        if (VirtualCursorUI != null && VirtualCursorUI.added)
        {
            InputSystem.RemoveDevice(VirtualCursorUI);
        }
        InputSystem.onAfterUpdate -= UpdateMotion;
        PlayerInput.onControlsChanged -= UpdateControlContext;
    }

    private void UpdateMotion()
    {
        if (VirtualCursorUI != null && Gamepad.current != null)
        {
            Vector2 StickValue = Gamepad.current.leftStick.ReadValue();
            StickValue *= VirtualCursorSpeed * Time.deltaTime;

            Vector2 CurrentPositon = VirtualCursorUI.position.ReadValue();
            Vector2 NewPostion = CurrentPositon + StickValue;

            // Limit cursor to screen
            NewPostion.x = Mathf.Clamp(NewPostion.x, HorizontalPadding, Screen.width - HorizontalPadding);
            NewPostion.y = Mathf.Clamp(NewPostion.y, VerticalPadding, Screen.height - VerticalPadding);

            // Apply cursor movement
            InputState.Change(VirtualCursorUI.position, NewPostion);
            InputState.Change(VirtualCursorUI.delta, StickValue);

            //TODO: Use new input system
            bool aButtonPressed = Gamepad.current.aButton.IsPressed();
            if (bPreviousCursorState != aButtonPressed)
            {
                VirtualCursorUI.CopyState<MouseState>(out var MouseState);
                MouseState.WithButton(MouseButton.Left, aButtonPressed);
                InputState.Change(VirtualCursorUI, MouseState);
                bPreviousCursorState = aButtonPressed;
            }

            AnchorCursor(NewPostion);
        }
    }

    private void AnchorCursor(Vector2 Positon)
    {
        Vector2 AnchoredPositon;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasTransform, Positon, 
            Canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : GameCamera, out AnchoredPositon);

        CursorInstanceTransfrom.anchoredPosition = AnchoredPositon;
    }

    private void UpdateControlContext(PlayerInput Input)
    {
        if (InputManager.IsCursorEnabled())
        {
            if (bEnaleDebugInfo)
            {
                Debug.Log("Virtual Cursor: " + Input.currentControlScheme);
            }
            
            if (Input.currentControlScheme == MouseControlScheme && PerviousControlScheme != MouseControlScheme)
            {
                SwitchToMouseCursor();
            }
            else if (Input.currentControlScheme == GamepadControlScheme && PerviousControlScheme != GamepadControlScheme)
            {
                SwitchToVirtualCursor();
            }
        }
    }

    private void SwitchToVirtualCursor()
    {
        CursorInstanceTransfrom.gameObject.SetActive(true);
        Cursor.visible = false;
        InputState.Change(VirtualCursorUI.position, CurrentMouse.position.ReadValue());
        AnchorCursor(CurrentMouse.position.ReadValue());
        PerviousControlScheme = GamepadControlScheme;
    }

    private void SwitchToMouseCursor()
    {
        CursorInstanceTransfrom.gameObject.SetActive(false);
        Cursor.visible = true;
        CurrentMouse.WarpCursorPosition(VirtualCursorUI.position.ReadValue());
        PerviousControlScheme = MouseControlScheme;
    }

    public Vector2 GetCursorPosition()
    {
        Vector2 Position = PlayerInput.currentControlScheme == MouseControlScheme 
            ? CurrentMouse.position.ReadValue() : VirtualCursorUI.position.ReadValue();

        return Position;
    }

    public void EnableCursor()
    {
        CursorInstanceTransfrom.gameObject.SetActive(true);
        if (bEnaleDebugInfo)
        {
            Debug.Log("Enable Virtual Curor");
        }
    }

    public void DisableCursor()
    {
        CursorInstanceTransfrom.gameObject.SetActive(false);
        if (bEnaleDebugInfo)
        {
            Debug.Log("Disable Virtual Curor");
        }
    }

    public void ModifyVirtualCursorSpeed(float InNewSpeed) => VirtualCursorSpeed = InNewSpeed;
    public float GetVirtualCursorSpeed() => VirtualCursorSpeed;
    public float GetDefaultCursorSpeed() => DefaultVirtualCursorSpeed;
}