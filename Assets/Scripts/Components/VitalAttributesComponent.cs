using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class VitalAttributesComponent : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float MaxCharacterHealth;
    [ShowInInspector, ReadOnly] private float CurrentCharacterHealth;

    [Header("Currency")]
    [ShowInInspector, ReadOnly] int Currency;

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
}
