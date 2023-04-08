using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;


public struct FRadialProgressSettings
{
    public enum ERadialType
    {
        Box,
        Circle,
    };

    public ERadialType RadialType;
    public bool bChangeByInputContext;
    // Progress will start filled and will progress down.
    public bool bReversed;
    public float HoldTime;
    // If true, progress will reset upon release
    public bool bResetOnStopHold;
    public bool bInstantReset;

    public FRadialProgressSettings(ERadialType InRadialType, bool bInChangeByInputContext, bool bInReversed, float InHoldTime, bool bInResetOnStopHold, 
        bool bInInstantReset)
    {
        RadialType = InRadialType;
        bChangeByInputContext = bInChangeByInputContext;
        bReversed = bInReversed;
        HoldTime = InHoldTime;
        bResetOnStopHold = bInResetOnStopHold;
        bInstantReset = bInInstantReset;
    }
};

public class RadialProgress : MonoBehaviour
{
    [SerializeField] private Slider Slider;
    [SerializeField] private GameObject BoxProgress;
    [SerializeField] private GameObject CircleProgress;

    [Header("Radial Settings")]
    [ShowInInspector] public FRadialProgressSettings RadialProgressSettings = new FRadialProgressSettings(); 

    private InputManager InputManager;
    private bool bIsHeld = false;

    private void OnEnable()
    {
        GetInputManager().InputContextChangedDelegate += OnInputContextChange;
    }

    private void OnDisable()
    {
        GetInputManager().InputContextChangedDelegate -= OnInputContextChange;
    }

    private void Update()
    {
        float Increment = Time.deltaTime / RadialProgressSettings.HoldTime;
        if (bIsHeld)
        {
            Slider.value += RadialProgressSettings.bReversed ? -Increment : Increment;
        }
        else if (RadialProgressSettings.bResetOnStopHold)
        {
            Slider.value += RadialProgressSettings.bReversed ? Increment : -Increment;
        }
    }

    public void SetRaidialProgressScale(Vector3 InNewScale)
    {
        Image RadialProgressImage = BoxProgress.activeSelf ? BoxProgress.GetComponent<Image>() : CircleProgress.GetComponent<Image>();
        RadialProgressImage.rectTransform.localScale += InNewScale;
    }

    private void OnInputContextChange(InputManager.EInputType NewInputContext, FUserSettings.EInputIconType NewIconType)
    {
        if (RadialProgressSettings.bChangeByInputContext)
        {
            FRadialProgressSettings.ERadialType RadialType = NewInputContext == InputManager.EInputType.Gamepad ? FRadialProgressSettings.ERadialType.Circle 
                : FRadialProgressSettings.ERadialType.Box;

            UpdateRadialType(RadialType);
        }
    }

    private void UpdateRadialType(FRadialProgressSettings.ERadialType InRadialType)
    {
        switch (InRadialType)
        {
            case FRadialProgressSettings.ERadialType.Box:
                {
                    BoxProgress.SetActive(true);
                    CircleProgress.SetActive(false);
                    Slider.image = BoxProgress.GetComponent<Image>();  
                    break;
                }
           case FRadialProgressSettings.ERadialType.Circle:
                {
                    CircleProgress.SetActive(true);
                    BoxProgress.SetActive(false);
                    Slider.image = CircleProgress.GetComponent<Image>();
                    break;
                }
            default:
                {
                    CircleProgress.SetActive(true);
                    BoxProgress.SetActive(false);
                    Slider.image = CircleProgress.GetComponent<Image>();
                    break;
                }
        }
    }

    private InputManager GetInputManager()
    {
        if (InputManager == null) InputManager = TempoManager.Instance.GetInputManager();
        return InputManager;
    }

    public void SetupRadialProgress(ref FRadialProgressSettings InSettings)
    {
        RadialProgressSettings = InSettings;
        Slider.value = InSettings.bReversed ? 1.0f : 0.0f;
        if (InSettings.bChangeByInputContext)
        {
            OnInputContextChange(GetInputManager().GetCurrentInputType(), FUserSettings.EInputIconType.Dynamic);
        }
        else
        {
            UpdateRadialType(InSettings.RadialType);
        }
    }

    public bool SetProgress(float InProgress)
    {
        Slider.value = InProgress;
        return HasCompleted();
    }

    public bool UpdateProgress(float InIncrementalAmount)
    {
        Slider.value += RadialProgressSettings.bReversed ? -InIncrementalAmount : InIncrementalAmount;
        return HasCompleted();
    }

    public bool HasCompleted()
    {
        return RadialProgressSettings.bReversed ? Slider.value <= 0.0f : Slider.value >= 1.0f;
    }

    public void StartHold()
    {
        bIsHeld = true;
    }

    public void EndHold()
    {
        bIsHeld= false;
        if (RadialProgressSettings.bInstantReset)
        {
            SetProgress(RadialProgressSettings.bReversed ? 1.0f : 0.0f);
        }
    }

    public bool IsHeld() => bIsHeld;
    public float GetProgress() => Slider.value;
}