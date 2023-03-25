using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(fileName = "DT_InputIcons", menuName = "Data/UI/Input Icons")]
public class DataTable_InputIcons : SerializedScriptableObject
{
    public struct FInputIcons
    {
        public string ActionName; // Name of input
        public Dictionary<FUserSettings.EInputIconType, Sprite> InputIconTypes;
    }

    public List<FInputIcons> LocalInputIcons = new List<FInputIcons>() { new FInputIcons() };

    public bool IsEmpty()
    {
        return LocalInputIcons.Count <= 0;
    }
    
    public bool IsValidRow(string InActionName)
    {
        foreach (FInputIcons InputIconRow in LocalInputIcons)
        {
            if (InputIconRow.ActionName.Equals(InActionName))
            {
                RowHasValidInfo(GetIndexByActionName(InActionName));
            }
        }
        return false;
    }
    
    public bool IsValidRow(int InIndex)
    {
        if (LocalInputIcons.Count >= InIndex)
        {
            RowHasValidInfo(InIndex);
        }
        return false;
    }
    
    private bool RowHasValidInfo(int InIndex)
    {
        if (GetInputIcon(InIndex) == null)
        {
            Debug.LogWarning("Row " + LocalInputIcons[InIndex].ActionName + " in " + name + " has an invalid input icon sprite");
            return false;
        }
        else
        {
            return true;
        }
    }
    
    public int GetIndexByActionName(string InActionName)
    {
        for (int Index = 0; Index < LocalInputIcons.Count; Index++)
        {
            if (LocalInputIcons[Index].ActionName.Equals(InActionName))
            {
                return Index;
            }
        }
        Debug.LogWarning("Unable to find valid row in " + name + " for action name: " + InActionName);
        return -1;
    }
    
    public string GetActionByIndex(int InIndex)
    {
        if (LocalInputIcons.Count <= InIndex)
        {
            return LocalInputIcons[InIndex].ActionName;
        }
        Debug.LogWarning("Unable to find valid input icon actoin row in " + name + " for index " + InIndex);
        return "";
    }
    
    public FInputIcons GetInputIconsRow(string InActionName)
    {
        if (IsValidRow(InActionName))
        {
            foreach (FInputIcons InputIconRow in LocalInputIcons)
            {
                if (InputIconRow.ActionName.Equals(InActionName))
                {
                    return InputIconRow;
                }
            }
        }
        Debug.LogWarning("No valid row found in " + name + " for action name: " + InActionName);
        return new FInputIcons();
    }
    
    public Sprite GetInputIcon(string InActionName)
    {
        if (IsValidRow(InActionName))
        {
            FUserSettings.EInputIconType IconType = TempoManager.Instance.GetGameUserSettings().GetInputIconType();
            Sprite Icon;
            if (GetInputIconsRow(InActionName).InputIconTypes.TryGetValue(IconType, out Icon))
            {
                return Icon;
            }
        }
        Debug.LogWarning("No valid sprite found in " + name + " for action name: " + InActionName);
        return null;
    }
    
    public Sprite GetInputIcon(int InIndex)
    {
        if (IsValidRow(InIndex))
        {
            return GetInputIcon(GetActionByIndex(InIndex));
        }
        Debug.LogWarning("No valid sprite found in " + name + " for index: " + InIndex);
        return null;
    }
}