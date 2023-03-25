using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETransitionType
{
    FadeIn, // Go to black
    FadeOut, // Go to gameplay
    FadeToScreen, // Go to black then to screen
    Instant, // Switch screens with no animation
}

public struct FTrasnistionSettings
{
    public MenuScreen Screen;
    public ETransitionType TransitionType;
    public float TransitionTime;
    public float Delay;
    public float HoldFadeTime;
}   


public class MenuManager : MonoBehaviour
{
    InputManager InputManager;

    MenuScreen CurrentActiveScreen = null;
    MenuScreen LastActiveScreen = null;
    FTrasnistionSettings DefaultSettings;

    [Header("Menu Library")]
    [SerializeField] string ActiveScreenName = "HUDScreen";
    [SerializeField] TempoMenuLibrary TempoMenuLibrary;

    [Header("Fade Screen")]
    [SerializeField] SpriteRenderer FadeScreen;
    [SerializeField] bool bFadeFromAwake = true;

    [Header("Debug")]
    [SerializeField] bool bDisplayDebugInfo = false;

    private void Awake()
    {
        FadeScreen.color = bFadeFromAwake ? new Color(0, 0, 0, 1) : new Color(0, 0, 0, 0);

        // Setup default transitioin settings
        DefaultSettings.TransitionTime = 1.0f;
        DefaultSettings.Delay = 0.0f;
        DefaultSettings.HoldFadeTime = 0.5f;
        DefaultSettings.TransitionType = ETransitionType.FadeOut;

        DisableAllMenuScreens();
    }

    private void Start()
    {
        InputManager = TempoManager.Instance.GetInputManager();

        FTrasnistionSettings ActiveScreenTransitionSettings = DefaultSettings;
        ActiveScreenTransitionSettings.Screen = FindScreenOfName(ActiveScreenName);
        if (ActiveScreenTransitionSettings.Screen)
        {
            ActiveScreenTransitionSettings.TransitionType = bFadeFromAwake ? ETransitionType.FadeToScreen : ETransitionType.Instant;
        }
        else
        {
            ActiveScreenTransitionSettings.TransitionType = bFadeFromAwake ? ETransitionType.FadeOut : ETransitionType.Instant;
        }
        StartTransitionToScreen(ActiveScreenTransitionSettings);
    }

    public void StartTransitionToScreen(FTrasnistionSettings Settings)
    {
        LastActiveScreen = CurrentActiveScreen;
        CurrentActiveScreen = Settings.Screen;
        
        if (CurrentActiveScreen)
        {
            CurrentActiveScreen.gameObject.SetActive(true);
        }

        if (bDisplayDebugInfo) Debug.Log("Transition From: " + LastActiveScreen + " Screen To: " + CurrentActiveScreen + " Screen");

        StartCoroutine(InternalTransition(Settings));
    }

    /// This should only ever be called through MenuManager::StartTransitionToScreen()
    private IEnumerator InternalTransition(FTrasnistionSettings Settings)
    {
        yield return new WaitForSeconds(Settings.Delay);

        float Incriment = (1 * Time.deltaTime) / Settings.TransitionTime;
        switch (Settings.TransitionType)
        {
            case ETransitionType.FadeIn:
                {
                    if (bDisplayDebugInfo) Debug.Log("Start Fade In");
                    for (float CurrentAlpha = FadeScreen.material.color.a; CurrentAlpha < 1.0f; CurrentAlpha += Incriment)
                    {
                         Color NewOpacity = new Color(FadeScreen.color.r, FadeScreen.color.g, FadeScreen.color.b, CurrentAlpha);
                         FadeScreen.material.color = NewOpacity;
                         yield return null;
                    }
                    break;
                }
            case ETransitionType.FadeOut:
                {
                    if (bDisplayDebugInfo) Debug.Log("Start Fade Out");
                    for (float CurrentAlpha = FadeScreen.material.color.a; CurrentAlpha > 0.0f; CurrentAlpha -= Incriment)
                    {
                        Color NewOpacity = new Color(FadeScreen.color.r, FadeScreen.color.g, FadeScreen.color.b, CurrentAlpha);
                        FadeScreen.material.color = NewOpacity;
                        yield return null;
                    }
                    OnTransitoinEnd(Settings);
                    break;
                }
            case ETransitionType.FadeToScreen:
                {
                    Settings.TransitionType = ETransitionType.FadeIn;
                    StartTransitionToScreen(Settings);
                    yield return new WaitForSeconds(Settings.TransitionTime);
                    if (bDisplayDebugInfo) Debug.Log("Start Fade Hold Wait");
                    yield return new WaitForSeconds(Settings.HoldFadeTime);
                    Settings.TransitionType = ETransitionType.FadeOut;
                    StartTransitionToScreen(Settings);
                    break;
                }
            case ETransitionType.Instant:
                {
                    FadeScreen.material.color = new Color(FadeScreen.color.r, FadeScreen.color.g, FadeScreen.color.b, 0);
                    OnTransitoinEnd(Settings);
                    break;
                }
            default:
            {
                Debug.LogError("Falied To Find Transition Type");
                break;
            }
        }
    }

    private void OnTransitoinEnd(FTrasnistionSettings Settings)
    {
        if (LastActiveScreen && LastActiveScreen != CurrentActiveScreen)
        {
            LastActiveScreen.SwitchAway();
            LastActiveScreen.gameObject.SetActive(false);
        }
        if (CurrentActiveScreen)
        {
            CurrentActiveScreen.gameObject.SetActive(true);
            CurrentActiveScreen.SwitchTo();

            switch (CurrentActiveScreen.GetVirtualCursorType())
            {
                case EVirtualCursorType.All:
                    {
                        InputManager.EnableCursor();
                        break;
                    }
                case EVirtualCursorType.NoCursor:
                    {
                        InputManager.DisableCursor();
                        break;
                    }
                default:
                    {
                        Debug.LogWarning("Transition Settings Has No Defined Logic for VirtualCursorType: " + CurrentActiveScreen.GetVirtualCursorType());
                        InputManager.DisableCursor();
                        break;
                    }
            }
        }
    }

    /// Handle pressed input for the current active screen. RetrunVal: has input been handled
    public bool OnActionDown(string InActionName)
    {
        // Handle current screen input
        if (CurrentActiveScreen)
        {
            return CurrentActiveScreen.OnActionDown(InActionName);
        }
        return false;
    }

    /// Handle released input for the current active screen. RetrunVal: has input been handled
    public bool OnActionUp(string InActionName)
    {
        if (CurrentActiveScreen)
        {
            return CurrentActiveScreen.OnActionUp(InActionName);
        }
        return false;
    }

    private void DisableAllMenuScreens()
    {
        for (int Index = TempoMenuLibrary.transform.childCount -1; Index >= 0; Index--)
        {
            TempoMenuLibrary.transform.GetChild(Index).gameObject.SetActive(false);
        }
    }

    private MenuScreen FindScreenOfName(string ScreenName)
    {
        for (int Index = TempoMenuLibrary.transform.childCount -1; Index >= 0; Index--)
        {
            if (TempoMenuLibrary.transform.GetChild(Index).name.Equals(ScreenName))
            {
                return TempoMenuLibrary.transform.GetChild(Index).GetComponent<MenuScreen>();
            }
        }
        Debug.LogError("Unable to find screen of name: " + ScreenName);
        return null;
    }

    /// Start transition to the hud screen using default settings
    public void StartTransitionToHUDScreen()
    {
        FTrasnistionSettings HUDcreenTransitionSettings = DefaultSettings;
        HUDcreenTransitionSettings.Screen = FindScreenOfName("HUDScreen");
        StartTransitionToScreen(HUDcreenTransitionSettings);
    }

    /// Start transition to the hud screen using given settings. Note: FTrasnistionSettings' menu screen will be ignored
    public void StartTransitionToHUDScreen(FTrasnistionSettings Settings)
    {
        Settings.Screen = FindScreenOfName("HUDScreen");
        StartTransitionToScreen(Settings);
    }

    public MenuScreen GetLastActiveScreen()
    {
        return LastActiveScreen;
    }

    public MenuScreen GetCurrentActiveScreen()
    {
        return CurrentActiveScreen;
    }

    public VirtualCursor GetVirtualCursor()
    {
        return TempoMenuLibrary.GetVirtualCursor();
    }

    public TempoMenuLibrary GetMenuLibrary()
    {
        return TempoMenuLibrary;
    }
}