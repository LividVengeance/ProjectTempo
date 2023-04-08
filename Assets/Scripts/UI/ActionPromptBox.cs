using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;


public class ActionPromptBox : SerializedMonoBehaviour
{
    [SerializeField] private ActionPrompt ActionPromptPrefab;
    [ShowInInspector] public List<FActionPrmoptSettings> ActionPrompts = new List<FActionPrmoptSettings>();
    private List<ActionPrompt> LocalActionPrompts = new List<ActionPrompt>();

    private void OnEnable()
    {
        LocalActionPrompts.Clear();
        foreach (FActionPrmoptSettings PromptSettings in ActionPrompts)
        {
            AddActionPrompt(PromptSettings);
        }
    }

    private void OnDisable()
    {
        ClearAcionPrompts();
    }

    public void AddActionPrompt(FActionPrmoptSettings InPromptSettings)
    {
        ActionPrompt NewPrompt = Instantiate(ActionPromptPrefab);
        NewPrompt.SetupActionPrompt(InPromptSettings);
        NewPrompt.transform.parent = transform;
        LocalActionPrompts.Add(NewPrompt);
    }

    public void RemoveActionPrompt(int InIndex)
    {
        if (LocalActionPrompts.Count <= InIndex)
        {
            ActionPrompt PromptToRemove = LocalActionPrompts[InIndex];
            LocalActionPrompts.Remove(PromptToRemove);
            Destroy(PromptToRemove);
        }
        Debug.LogWarning("Unable to find action prompt at index " + InIndex);
    }

    public void RemoveActionPrompt(string InActionName)
    {
        foreach (ActionPrompt Prompt in LocalActionPrompts)
        {
            if (Prompt.ActionPromptSettings.InputActionName.Equals(InActionName))
            {
                Destroy(Prompt);
                LocalActionPrompts.Remove(Prompt);
                return;
            }
        }
        Debug.LogWarning("Failed to find action prompt with action name " + InActionName);
    }

    public void ClearAcionPrompts()
    {
        foreach (ActionPrompt Prompt in LocalActionPrompts)
        {
            Destroy(Prompt);
            LocalActionPrompts.Remove(Prompt);
        }
        LocalActionPrompts.Clear();
    }

    public List<ActionPrompt> GetActionPrompts() => LocalActionPrompts;
}