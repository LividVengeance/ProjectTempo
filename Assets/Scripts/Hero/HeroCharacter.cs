using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCharacter : TempoCharacter
{
    [Header("Player Controller")]
    [SerializeField] private HeroPlayerController HeroPlayerController;

    [Header("Components")]
    [SerializeField] private InventoryComponent InventoryComponent;
    [SerializeField] private ActionSystemComponent ActionSystemComponent;
    [SerializeField] private VitalAttributesComponent VitalAttributesComponent;
    [SerializeField] private ProgressionComponent ProgressionComponent;

    private void Start()
    {
        if (!InventoryComponent) Debug.LogError("No Inventory Component Has Been Assigned To " + gameObject.name);
        if (!ActionSystemComponent) Debug.LogError("No Action System Component Has Been Assigned To " + gameObject.name);
    }

    public InventoryComponent GetHeroInventoryComponent() => InventoryComponent;
    public VitalAttributesComponent GetVitalAttributesComponent() => VitalAttributesComponent;
    public ProgressionComponent GetProgressionComponent() => ProgressionComponent;
    public HeroPlayerController GetHeroController() => HeroPlayerController;
}
