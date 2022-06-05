using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class VitalAttributesComponent : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float MaxCharacterHealth;
    [ShowInInspector, ReadOnly] private float CurrentCharacterHealth;

    [Header("Armour Layers")]
    [SerializeField] private float MaxLayerHealth;
    [SerializeField] private int NumberOfArmourLayers = 0;
    private List<FArmourLayer> ArmourLayers = new List<FArmourLayer>();

    public struct FArmourLayer
    {
        public float MaxLayerHealth;
        public float CurrentLayerHealth;
    };

    private void Awake()
    {
        for (int i =0; i < NumberOfArmourLayers; i++)
        {
            FArmourLayer NewArmourLayer = new FArmourLayer();
            NewArmourLayer.MaxLayerHealth = MaxLayerHealth;
            ArmourLayers.Add(NewArmourLayer);
        }
    }

    public void PreProcessDamage()
    {

    }

    public bool ProcessDamage()
    {
        return false; // Won't take damage
    }

    public void PostProcessDamage()
    {

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

    public float GetCharacterCurrentHealth() => CurrentCharacterHealth;
    public float GetCharacterMaxHealth() => MaxCharacterHealth;
    public int GetNumberOfArmourLayers() => ArmourLayers.Count;
}
