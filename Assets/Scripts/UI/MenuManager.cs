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

    [SerializeField] SpriteRenderer FadeScreen;

    [SerializeField] bool bFadeFromAwake = true;

    [Header("Debug")]
    [SerializeField] bool bDisplayDebugInfo = false;

    private void Awake()
    {
        FadeScreen.color = bFadeFromAwake ? new Color(0, 0, 0, 1) : new Color(0, 0, 0, 0);

        // Setup default transitioin settings
        DefaultSettings.TransitionTime = 1.0f;
        DefaultSettings.Delay = 2.5f;
        DefaultSettings.TransitionType = ETransitionType.FadeOut;
    }

    private void Start()
    {
        InputManager = TempoManager.Instance.GetInputManager();

        if (bFadeFromAwake)
        {
            StartCoroutine(InternalTransition(DefaultSettings));
        }
    }

    public void StartTransitionToScreen(FTrasnistionSettings Settings)
    {
        LastActiveScreen = CurrentActiveScreen;
        CurrentActiveScreen = Settings.Screen;

        if (bDisplayDebugInfo) Debug.Log("Transition From: " + LastActiveScreen + " Screen To: " + CurrentActiveScreen + " Screen");

        StartCoroutine(InternalTransition(Settings));
    }

    IEnumerator InternalTransition(FTrasnistionSettings Settings)
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

    void OnTransitoinEnd(FTrasnistionSettings Settings)
    {
        if (LastActiveScreen) LastActiveScreen.gameObject.SetActive(false);
        if (CurrentActiveScreen)
        {
            CurrentActiveScreen.gameObject.SetActive(true);

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

    MenuScreen GetLastActiveScreen()
    {
        return LastActiveScreen;
    }

    MenuScreen GetCurrentActiveScreen()
    {
        return CurrentActiveScreen;
    }
}