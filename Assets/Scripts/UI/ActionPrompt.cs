using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;

public class ActionPrompt : SerializedMonoBehaviour
{  
    public struct FActionPrmoptSettings
    {
        public string InputActionName;
        public string InputText;
        public bool bHold;
        private float HoldTime;
        public bool bEnlargeOnHover;
        private float HoverEnlargeSize;
        public bool bClickable;

        FActionPrmoptSettings(string InInputActionName, string InInputText, bool bInHold, float InHoldTime, bool bInEnlargeOnHover, bool bInClickable)
        {
            InputActionName = InInputActionName;
            InputText = InInputText;
            bHold = bInHold;
            HoldTime = TempoManager.Instance.GetGameUserSettings().GetActionPromptHoldTime();
            bEnlargeOnHover = bInEnlargeOnHover;
            HoverEnlargeSize = TempoManager.Instance.GetGameSystemSettings().GetActionPromptEnlargeSize();
            bClickable = bInClickable;
        }
        
        public float GetActionPromptHoldTime => HoldTime;
        public float GetActionPromptHoverEnlargeSize => HoverEnlargeSize;
    };

    delegate void ActionPromptDelegate(string InActionName);
    private ActionPromptDelegate PromptTriggered;
    private ActionPromptDelegate PromptHovered;
    private ActionPromptDelegate PromptUnhovered;

    private Coroutine HoldTimer;
    private bool bIsHeld = false;

    [SerializeField] private TextMeshProUGUI InputText;
    [SerializeField] private TempoButton ActionPromptButton;
    [SerializeField] private InputIcon InputIcon;
    [ShowInInspector] public FActionPrmoptSettings ActionPromptSettings = new FActionPrmoptSettings();

    private void OnEnable()
    {
        ActionPromptButton.PointerDown += OnPromptClicked;
        ActionPromptButton.PointerEnter += OnPromptHovered;
        ActionPromptButton.PointerExit += OnPromptUnhovered;

        SetupActionPrompt(ActionPromptSettings);
    }

    public void SetupActionPrompt(FActionPrmoptSettings InPromptSettings)
    {
        ActionPromptSettings = InPromptSettings;
        InputIcon.SetIconBrush(InPromptSettings.InputActionName);
        InputText.SetText(InPromptSettings.InputText);
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
                PromptTriggered.Invoke(ActionPromptSettings.InputActionName);
            }
        }
    }

    private void OnPromptHovered(PointerEventData eventData)
    {
        if (ActionPromptSettings.bEnlargeOnHover)
        {
            Vector2 ImageScalar = new Vector2(ActionPromptSettings.GetActionPromptHoverEnlargeSize, ActionPromptSettings.GetActionPromptHoverEnlargeSize);
            ActionPromptButton.SetImageSize(ImageScalar);
        }

        if (PromptHovered != null)
        {
            PromptHovered.Invoke(ActionPromptSettings.InputActionName);
        }
    }

    private void OnPromptUnhovered(PointerEventData eventData)
    {
        if (ActionPromptSettings.bEnlargeOnHover)
        {
            Vector2 ImageScalar = new Vector2(-ActionPromptSettings.GetActionPromptHoverEnlargeSize, -ActionPromptSettings.GetActionPromptHoverEnlargeSize);
            ActionPromptButton.SetImageSize(ImageScalar);
        }

        if (PromptUnhovered != null)
        {
            PromptUnhovered.Invoke(ActionPromptSettings.InputActionName);
        }
    }

    private IEnumerator StartPromptHold()
    {
        bIsHeld = true;
        yield return new WaitForSeconds(ActionPromptSettings.GetActionPromptHoldTime);
        PromptTriggered.Invoke(ActionPromptSettings.InputActionName);
        bIsHeld = false;
    }

    public bool IsHeld => bIsHeld;
    public Coroutine GetHoldTimer => HoldTimer;
}
