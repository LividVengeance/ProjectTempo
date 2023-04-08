using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FUserSettings
{
    public enum EInputIconType
    {
        Dynamic, // Will use icons based on detected input hardware
        KeyboardMouse,
        Playstation,
        Xbox,
        Switch,
        GamepadGeneric,
    }

    public EInputIconType InputIconType;

    FUserSettings(EInputIconType InInputIconType)
    {
        InputIconType = InInputIconType;
    }

    EInputIconType GetCurrentInputType()
    {
        return InputIconType;
    }
};

public class TempoGameUserSettings : MonoBehaviour
{
    private FUserSettings CurrentUserSettings;
    private float ActionPromptHoldTime = 3.0f;

    public FUserSettings.EInputIconType GetInputIconType()
    {
        return CurrentUserSettings.InputIconType;
    }

    public float GetActionPromptHoldTime()
    {
        return ActionPromptHoldTime;
    }
}
