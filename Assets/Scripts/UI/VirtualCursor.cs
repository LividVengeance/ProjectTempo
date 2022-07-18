using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;

public class VirtualCursor : MonoBehaviour
{
    [SerializeField] private PlayerInput PlayerInput;
    private Mouse VirtualCursorUI = null;
    [SerializeField]
    private RectTransform CursorInstanceTransfrom;
    private float VirtualCursorSpeed = 1000.0f;
    private bool bPreviousCursorState;
    [SerializeField] RectTransform CanvasTransform;
    [SerializeField] Camera GameCamera;
    [SerializeField] Canvas Canvas;

    private Mouse CurrentMouse;

    [SerializeField] float VerticalPadding = 35.0f;
    [SerializeField] float HorizontalPadding = 35.0f;

    private string PerviousControlScheme = "";
    private const string GamepadControlScheme = "Gamepad";
    private const string MouseControlScheme = "Keyboard&Mouse";

    private void OnEnable()
    {
        GameCamera = Camera.main;
        CurrentMouse = Mouse.current;

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
        Debug.Log(Input.currentControlScheme);
        if (Input.currentControlScheme == MouseControlScheme && PerviousControlScheme != MouseControlScheme)
        {
            CursorInstanceTransfrom.gameObject.SetActive(false);
            //TODO: Should tie into the input system i've made
            Cursor.visible = true;
            CurrentMouse.WarpCursorPosition(VirtualCursorUI.position.ReadValue());
            PerviousControlScheme = MouseControlScheme;
        }
        else if (Input.currentControlScheme == GamepadControlScheme && PerviousControlScheme != GamepadControlScheme)
        {
            CursorInstanceTransfrom.gameObject.SetActive(true);
            //TODO: Should tie into the input system i've made
            Cursor.visible = false;
            InputState.Change(VirtualCursorUI.position, CurrentMouse.position.ReadValue());
            AnchorCursor(CurrentMouse.position.ReadValue());
            PerviousControlScheme = GamepadControlScheme;
        }
    }
}
