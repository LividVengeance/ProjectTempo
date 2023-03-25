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

    private MenuManager OwningMenuManager;

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

        OwningMenuManager = TempoManager.Instance.GetMenuManager();
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
        // Handle in child
    }

    public virtual void SwitchAway()
    {
        // Handle in child
    }

    public virtual bool OnActionDown(string InActionName)
    {
        // Handle in child
        return false;
    }

    public virtual bool OnActionUp(string InActionName)
    {
        // Handle in child
        return false;
    }

    public MenuManager GetMenuManager() => OwningMenuManager;
}
