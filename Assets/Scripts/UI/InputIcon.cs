using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputIcon : Image
{
    [SerializeField] string ActionName;
    private DataTable_InputIcons InputIconsTable;
    private Dictionary<FUserSettings.EInputIconType, Sprite> InputIcons = new Dictionary<FUserSettings.EInputIconType, Sprite>();

    protected override void OnEnable()
    {
        base.OnEnable();
        if (Application.isPlaying)
        {
            TempoManager.Instance.GetInputManager().InputContextChangedDelegate += OnInputContextChange;
        }
        InputIconsTable = TempoManager.Instance.GetGameSystemSettings().GetInputIcons();
        UpdatedCahcedInputIcons();
    }

    private void OnInputContextChange(InputManager.EInputType InNewInputType, FUserSettings.EInputIconType InNewInputIcon)
    {
        Sprite NewInputIcon;
        if (InputIcons.TryGetValue(InNewInputIcon, out NewInputIcon))
        {
            sprite = NewInputIcon;
        }
    }

    private void UpdatedCahcedInputIcons()
    {
        InputIcons.Clear();
        int InputTypesCount = FUserSettings.EInputIconType.GetNames(typeof(FUserSettings.EInputIconType)).Length;
        for (int Index = 0; Index < InputTypesCount; Index++)
        {
            InputIconsTable = TempoManager.Instance.GetGameSystemSettings().GetInputIcons();
            InputIcons.Add((FUserSettings.EInputIconType)Index, InputIconsTable.GetInputIcon(ActionName, (FUserSettings.EInputIconType)Index));
        }
    }

    public void SetIconBrush(string InActionName)
    {
        UpdatedCahcedInputIcons();
        ActionName = InActionName;

        Sprite NewInputIcon;
        FUserSettings.EInputIconType IconType = TempoManager.Instance.GetInputManager().GetIconType();
        if (InputIcons.TryGetValue(IconType, out NewInputIcon))
        {
            sprite = NewInputIcon;
        }
    }
}
