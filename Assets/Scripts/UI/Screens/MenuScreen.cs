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
    [SerializeField] private string ScreenName;
    [SerializeField] private EVirtualCursorType VirtualCursorType;
    [SerializeField] private ActionPromptBox ActivePromptBox = null;

    private MenuManager OwningMenuManager;

    public FTrasnistionSettings DefaultTransitionSettings;
    public FTrasnistionSettings InstantTransitionSettings;

    public MenuScreen()
    {
        // Default Transition Settings
        DefaultTransitionSettings = new FTrasnistionSettings(this, ETransitionType.FadeToScreen, 0.25f, 0.0f, 0.0f);

        // Instant Transition Settings
        InstantTransitionSettings = new FTrasnistionSettings(this, ETransitionType.Instant, 0.15f, 0.0f, 0.0f);
    }

    private void Start()
    {
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
        bool bInputHandled = false;
        if (ActivePromptBox)
        {
            foreach (ActionPrompt Prompt in ActivePromptBox.GetActionPrompts())
            {
                if (Prompt.OnActionDown(InActionName))
                {
                    bInputHandled = true;
                    break;
                }
            }
        }
        return bInputHandled;
    }

    public virtual bool OnActionUp(string InActionName)
    {
        bool bInputHandled = false;
        if (ActivePromptBox)
        {
            foreach (ActionPrompt Prompt in ActivePromptBox.GetActionPrompts())
            {
                if (Prompt.OnActionUp(InActionName))
                {
                    bInputHandled = true;
                    break;
                }
            }
        }
        return bInputHandled;
    }

    public MenuManager GetMenuManager() => OwningMenuManager;
}
