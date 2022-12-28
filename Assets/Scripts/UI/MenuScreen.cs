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

    public FTrasnistionSettings DefaultTransitionSettings;

    private void Start()
    {
        DefaultTransitionSettings.Delay = 0.0f;
        DefaultTransitionSettings.HoldFadeTime = 0.0f;
        DefaultTransitionSettings.Screen = this;
        DefaultTransitionSettings.TransitionTime = 0.25f;
        DefaultTransitionSettings.TransitionType = ETransitionType.FadeToScreen;
    }

    public string GetMenuName()
    {
        return ScreenName;
    }

    public EVirtualCursorType GetVirtualCursorType()
    {
        return VirtualCursorType;
    }
}
