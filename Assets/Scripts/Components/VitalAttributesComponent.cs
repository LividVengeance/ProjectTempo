using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class VitalAttributesComponent : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private ActionSystemComponent ActionSystem;

    [Header("Health")]
    [SerializeField] private float MaxCharacterHealth;
    [ShowInInspector, ReadOnly] private float CurrentCharacterHealth;

    [Header("Armour Layers")]
    [SerializeField] private float MaxLayerHealth;
    [SerializeField] private int NumberOfArmourLayers = 0;
    private List<FArmourLayer> ArmourLayers = new List<FArmourLayer>();

    [Header("Invulnerability")]
    [SerializeField, ReadOnly] private bool bGodMode = false;
    [SerializeField, ReadOnly] private bool bTitanMode = false;
    [SerializeField, ReadOnly] private int InvulnerabilityStack;
    [SerializeField, ReadOnly] List<string> InvulnerabilityHistory = new List<string>();

    [Header("Applied Modifiers")]
    [SerializeField, ReadOnly] List<DamageModifier> AppliedDamageModifiers = new List<DamageModifier>();

    public struct FArmourLayer
    {
        public float MaxLayerHealth;
        public float CurrentLayerHealth;
    };

    private void Awake()
    {
        for (int i = 0; i < NumberOfArmourLayers; i++)
        {
            FArmourLayer NewArmourLayer = new FArmourLayer();
            NewArmourLayer.MaxLayerHealth = MaxLayerHealth;
            ArmourLayers.Add(NewArmourLayer);
        }

        CurrentCharacterHealth = MaxCharacterHealth;
    }

    public void PreProcessDamage()
    {

    }

    public bool ProcessDamage()
    {
        if (bGodMode)
        {
            return false;
        }
        return false; // Won't take damage
    }

    private void Update()
    {
        PostProcessDamage();
    }

    public void PostProcessDamage()
    {
        // Start the death action if has no health
        if (CurrentCharacterHealth <= 0 && !ActionSystem.IsCurrentAction(ActionSystem.DeathActionState))
        {
            ActionSystem.ChangeAction(ActionSystem.DeathActionState);
            Debug.Log(transform.parent.gameObject + " : Started death action");
        }
    }

    public bool HasAnyArmourPercentage()
    {
        foreach(FArmourLayer CurrentArmourLayer in ArmourLayers)
        {
            if (CurrentArmourLayer.CurrentLayerHealth > 0)
            {
                return true;
            }
        }
        return false;
    }

    public float GetAllArmourLayerHealth()
    {
        float TotalArmourHealth = 0.0f;
        foreach (FArmourLayer CurrentArmourLayer in ArmourLayers)
        {
            TotalArmourHealth = CurrentArmourLayer.CurrentLayerHealth;
        }
        return TotalArmourHealth;
    }

    public void AddInvulnerabilityStack(string SourceName)
    {
        InvulnerabilityStack++;
        InvulnerabilityHistory.Add("Added: " + SourceName);
    }

    public void RemoveInvulnerabilityStack(string SourceName)
    {
        InvulnerabilityStack--;
        InvulnerabilityHistory.Add("Removed: " + SourceName);
    }

    public float GetCharacterCurrentHealth() => CurrentCharacterHealth;
    public float GetCharacterMaxHealth() => MaxCharacterHealth;
    public int GetNumberOfArmourLayers() => ArmourLayers.Count;

    public bool GetGodModeState() => bGodMode;
    public void SetGodModeState(bool bEnable) => bGodMode = bEnable;

    public bool GetTitanModeState() => bTitanMode;
    public void SetTitanModeState(bool bEnable) => bTitanMode = bEnable; 
}
