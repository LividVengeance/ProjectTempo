using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempoGameSystemSettings : MonoBehaviour
{
    [SerializeField] private DataTable_InputIcons InputIcons;

    [Header("Action Prompt Settings")]
    [SerializeField] private float ActionPromptEnlargeSize = 1.5f;

    public DataTable_InputIcons GetInputIcons()
    {
        return InputIcons;
    }

    public float GetActionPromptEnlargeSize()
    {
        return ActionPromptEnlargeSize;
    }
}
