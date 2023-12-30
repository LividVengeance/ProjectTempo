using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using static DataTable_InputIcons;

[CreateAssetMenu(fileName = "DT_InputIcons", menuName = "Data/UI/Input Icons")]
public class DataTable_InputIcons : SerializedScriptableObject
{
    public struct FInputIcons
    {
        public string ActionName; // Name of input
        public List<FInputIconInfo> InputIconInfo;

        public FInputIcons(string InActionName)
        {
            ActionName = InActionName;
            InputIconInfo = new List<FInputIconInfo>();
        }

        public bool IsDataValid()
        {
            return !ActionName.Equals("") && InputIconInfo.Count > 0;
        }
    }

    public struct FInputIconInfo
    {
        public FUserSettings.EInputIconType IconType;
        public Sprite IconSprite;
        public FRadialProgressSettings.ERadialType RadialType;
    }

    [SerializeField] public List<FInputIcons> LocalInputIcons = new List<FInputIcons>() { new FInputIcons() };
    [Header("Debug")]
    [SerializeField] bool bDisplayDegbug = false;

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
        if (GetInputIconForCurrentInputType(InIndex) == null)
        {
            if (bDisplayDegbug)
            {
                Debug.LogWarning("Row " + LocalInputIcons[InIndex].ActionName + " in " + name + " has an invalid input icon sprite");
            }
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
        if (bDisplayDegbug)
        {
            Debug.LogWarning("Unable to find valid row in " + name + " for action name: " + InActionName);
        }
        return -1;
    }
    
    public string GetActionByIndex(int InIndex)
    {
        if (LocalInputIcons.Count <= InIndex)
        {
            return LocalInputIcons[InIndex].ActionName;
        }
        if (bDisplayDegbug)
        {
            Debug.LogWarning("Unable to find valid input icon actoin row in " + name + " for index " + InIndex);
        }
        return "";
    }
    
    public FInputIcons GetInputIconsRow(string InActionName)
    {
        foreach (FInputIcons InputIconRow in LocalInputIcons)
        {
            if (InputIconRow.ActionName.Equals(InActionName))
            {
                return InputIconRow;
            }
        }
        if (bDisplayDegbug)
        {
            string DebugString = InActionName.Equals("") ? "EmptyActionName" : InActionName;
            Debug.LogWarning("No valid row found in " + name + " for action name: " + DebugString);
        }
        return new FInputIcons();
    }
    
    public Sprite GetInputIconForCurrentInputType(string InActionName)
    {
        FUserSettings.EInputIconType IconType = TempoManager.Instance.GetGameUserSettings().GetInputIconType();
        FInputIcons InputIcons = GetInputIconsRow(InActionName);
        foreach (FInputIconInfo IconInfo in InputIcons.InputIconInfo)
        {
            if (IconInfo.IconType == IconType)
            {
                return IconInfo.IconSprite;
            }
        }
        if (bDisplayDegbug)
        {
            string DebugString = InActionName.Equals("") ? "EmptyActionName" : InActionName;
            Debug.LogWarning("No valid sprite found in " + name + " for action name: " + DebugString);
        }
        return null;
    }
    
    public Sprite GetInputIconForCurrentInputType(int InIndex)
    {
        return GetInputIconForCurrentInputType(GetActionByIndex(InIndex));
    }

    public Sprite GetInputIcon(string InActionName, FUserSettings.EInputIconType InInputType)
    {
        FInputIcons InputIcons = GetInputIconsRow(InActionName);
        if (InputIcons.IsDataValid())
        {
            foreach (FInputIconInfo IconInfo in InputIcons.InputIconInfo)
            {
                if (IconInfo.IconType == InInputType)
                {
                    return IconInfo.IconSprite;
                }
            }
        }
        return null;
    }

    public FInputIconInfo GetInputIconInfo(string InActionName, FUserSettings.EInputIconType InInputType)
    {
        FInputIcons InputIcons = GetInputIconsRow(InActionName);

        foreach (FInputIconInfo IconInfo in InputIcons.InputIconInfo)
        {
            if (IconInfo.IconType == InInputType)
            {
                return IconInfo;
            }
        }
        return new FInputIconInfo();
    }
}