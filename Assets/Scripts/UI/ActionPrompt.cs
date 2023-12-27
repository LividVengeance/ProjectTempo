using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

public struct FActionPrmoptSettings
{
    public string InputActionName;
    public string InputText;
    public bool bHold;
    public float HoldTime;
    public bool bEnlargeOnHover;
    public float HoverEnlargeSize;
    public bool bClickable;

    // TODO: Have default values be pulled from system/user data - have them set back to private
    public FActionPrmoptSettings(string InInputActionName, string InInputText, bool bInHold, float InHoldTime, bool bInEnlargeOnHover, bool bInClickable)
    {
        InputActionName = InInputActionName;
        InputText = InInputText;
        bHold = bInHold;
        //HoldTime = TempoManager.Instance.GetGameUserSettings().GetActionPromptHoldTime();
        HoldTime = 3.0f;
        bEnlargeOnHover = bInEnlargeOnHover;
        //HoverEnlargeSize = TempoManager.Instance.GetGameSystemSettings().GetActionPromptEnlargeSize();
        HoverEnlargeSize = 1.5f;
        bClickable = bInClickable;
    }

    public float GetActionPromptHoldTime => HoldTime;
    public float GetActionPromptHoverEnlargeSize => HoverEnlargeSize;
};

public class ActionPrompt : SerializedMonoBehaviour
{
    delegate void ActionPromptDelegate(string InActionName);
    private ActionPromptDelegate PromptTriggered;
    private ActionPromptDelegate PromptHovered;
    private ActionPromptDelegate PromptUnhovered;

    private Coroutine HoldTimer;
    private bool bIsHeld = false;
    private bool bAppliedScale = false;

    [SerializeField] private TextMeshProUGUI InputText;
    [SerializeField] private TempoButton ActionPromptButton;
    [SerializeField] private InputIcon InputIcon;
    [SerializeField] private RadialProgress RadialProgress;
    [SerializeField] HorizontalLayoutGroup LayoutGroup;

    [Space]
    [ShowInInspector] public FActionPrmoptSettings ActionPromptSettings = new FActionPrmoptSettings();


    void OnEnable()
    {
        //TODO: Remove when pulling from system/user data
        ActionPromptSettings.HoldTime = 1.0f;
        ActionPromptSettings.HoverEnlargeSize = 0.3f;

        ActionPromptButton.PointerDown += OnPromptClicked;
        ActionPromptButton.PointerUp += OnPromptReleased;
        ActionPromptButton.PointerEnter += OnPromptHovered;
        ActionPromptButton.PointerExit += OnPromptUnhovered;
        
        SetupActionPrompt(ActionPromptSettings);
    }

    private void OnDisable()
    {
        ActionPromptButton.PointerDown -= OnPromptClicked;
        ActionPromptButton.PointerUp -= OnPromptReleased;
        ActionPromptButton.PointerEnter -= OnPromptHovered;
        ActionPromptButton.PointerExit -= OnPromptUnhovered;
    }

    /// Input should be handed off from MenuScreens
    public bool OnActionDown(string InActionName)
    {
        bool bInputHandled = false;
        if (InActionName == ActionPromptSettings.InputActionName)
        {
            OnPromptClicked(new PointerEventData(EventSystem.current));
            bInputHandled = true;
        }
        return bInputHandled;
    }

    /// Input should be handed off from MenuScreens
    public bool OnActionUp(string InActionName)
    {
        bool bInputHandled = false;
        if (InActionName == ActionPromptSettings.InputActionName)
        {
            OnPromptReleased(new PointerEventData(EventSystem.current));
            bInputHandled = true;
        }
        return bInputHandled;
    }

    public void SetupActionPrompt(FActionPrmoptSettings InPromptSettings)
    {
        ActionPromptSettings = InPromptSettings;
        InputIcon.SetIconBrush(InPromptSettings.InputActionName);
        InputText.SetText(InPromptSettings.InputText);

        RadialProgress.gameObject.SetActive(ActionPromptSettings.bHold);
        if (ActionPromptSettings.bHold)
        {
            FRadialProgressSettings RadialSettings = new FRadialProgressSettings(FRadialProgressSettings.ERadialType.Box, true, false, 
                ActionPromptSettings.GetActionPromptHoldTime, true, false);

            RadialProgress.SetupRadialProgress(ref RadialSettings);
        }
    }

    private void OnPromptClicked(PointerEventData eventData)
    {
        if (ActionPromptSettings.bClickable)
        {
            if (ActionPromptSettings.bHold)
            {
                HoldTimer = StartCoroutine(StartPromptHold());
            }
            else
            {
                OnTrigger();
            }
        }
    }

    private void OnPromptReleased(PointerEventData eventData)
    {
        if (ActionPromptSettings.bClickable)
        {
            if (ActionPromptSettings.bHold)
            {
                EndPromptHold();
            }
        }
    }

    private void OnPromptHovered(PointerEventData eventData)
    {
        if (ActionPromptSettings.bEnlargeOnHover && !bAppliedScale)
        {
            Vector3 ImageScalar = new Vector3(ActionPromptSettings.GetActionPromptHoverEnlargeSize, ActionPromptSettings.GetActionPromptHoverEnlargeSize, 1.0f);
            ActionPromptButton.SetImageSize(ImageScalar);
            RadialProgress.SetRaidialProgressScale(ImageScalar);
            bAppliedScale = true;
        }

        if (PromptHovered != null)
        {
            PromptHovered.Invoke(ActionPromptSettings.InputActionName);
        }
    }

    private void OnPromptUnhovered(PointerEventData eventData)
    {
        if (ActionPromptSettings.bEnlargeOnHover && bAppliedScale)
        {
            Vector3 ImageScalar = new Vector3(-ActionPromptSettings.GetActionPromptHoverEnlargeSize, -ActionPromptSettings.GetActionPromptHoverEnlargeSize, 1.0f);
            ActionPromptButton.SetImageSize(ImageScalar);
            RadialProgress.SetRaidialProgressScale(ImageScalar);
            bAppliedScale = false;
        }

        if (PromptUnhovered != null)
        {
            PromptUnhovered.Invoke(ActionPromptSettings.InputActionName);
        }
    }

    private IEnumerator StartPromptHold()
    {
        bIsHeld = true;
        RadialProgress.StartHold();
        yield return new WaitForSeconds(ActionPromptSettings.GetActionPromptHoldTime);
        OnTrigger();
        bIsHeld = false;
    }

    private void EndPromptHold()
    {
        bIsHeld = false;
        RadialProgress.EndHold();
        StopCoroutine(HoldTimer);
    }

    public void SetPadding(RectOffset InPadding)
    {
        LayoutGroup.padding = InPadding;
        LayoutRebuilder.MarkLayoutForRebuild(GetComponent<RectTransform>());
    }    

    private void OnTrigger(bool bResetRadialProgress = true)
    {
        PromptTriggered?.Invoke(ActionPromptSettings.InputActionName);
        if (bResetRadialProgress)
        {
            RadialProgress.ResetProgress();
        }
    }

    public bool IsHeld => bIsHeld;
    public Coroutine GetHoldTimer => HoldTimer;
}