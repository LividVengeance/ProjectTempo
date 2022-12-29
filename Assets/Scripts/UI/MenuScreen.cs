using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EVirtualCursorType
{
    NoCursor,
    ControllerOnly,
    All,
}

public class MenuScreen : MonoBehaviour
{
    [SerializeField] string ScreenName;
    [SerializeField] EVirtualCursorType VirtualCursorType;

    private GameHUD GameHud;

    public FTrasnistionSettings DefaultTransitionSettings;
    public FTrasnistionSettings InstantTransitionSettings;

    private void Start()
    {
        // Default Transition Settings
        DefaultTransitionSettings.Delay = 0.0f;
        DefaultTransitionSettings.HoldFadeTime = 0.0f;
        DefaultTransitionSettings.Screen = this;
        DefaultTransitionSettings.TransitionTime = 0.25f;
        DefaultTransitionSettings.TransitionType = ETransitionType.FadeToScreen;

        // Instant Transition Settings
        InstantTransitionSettings.Delay = 0.0f;
        InstantTransitionSettings.TransitionTime = 0.15f;
        InstantTransitionSettings.TransitionType = ETransitionType.Instant;
        InstantTransitionSettings.Screen = this;
        InstantTransitionSettings.HoldFadeTime = 0.0f;

        GameHud = TempoManager.Instance.GetGameHUD();
    }

    public string GetMenuName()
    {
        return ScreenName;
    }

    public EVirtualCursorType GetVirtualCursorType()
    {
        return VirtualCursorType;
    }

    public virtual void SwitchTo()
    {

    }

    public virtual void SwitchAway()
    {

    }

    public GameHUD GetGameHUD() => GameHud;
}
