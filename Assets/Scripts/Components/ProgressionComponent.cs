using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class ProgressionComponent : MonoBehaviour
{
    // Events
    private UnityEvent CharacterLevelChangeEvent;

    [ShowInInspector, ReadOnly] 
    private int CurrentExperience;
    [SerializeField, Tooltip("The max amount of experience this character can hold. Keep 0 to have no cap")] 
    private int MaxExperienceCap;
    [ShowInInspector, ReadOnly] 
    private int CurrentLevel;
    [SerializeField, Tooltip("The maxium level this charater can be. Keep 0 to have no cap")] 
    private int MaxLevelCap;

    public void ModifyCharacterExperience(int ModifyAmount)
    {
        if (CurrentExperience + ModifyAmount > MaxExperienceCap && MaxLevelCap != 0)
        {
            Debug.LogWarning(transform.parent.gameObject + " : Unable To Apply All Experience - Hit Max Experience Cap");
            CurrentExperience = MaxExperienceCap;
        }
        else if (CurrentLevel + ModifyAmount < 0)
        {
            Debug.LogWarning(transform.parent.gameObject + " : Unable To Apply All Experience - Hit 0 Experience");
            CurrentLevel = 0;
        }
        else
        {
            CurrentExperience += ModifyAmount;
        }
    }

    public void ModifyCharacterMaxLevelCap(int ModifyAmount)
    {
        if (MaxLevelCap + ModifyAmount < 1)
        {
            Debug.LogWarning(transform.parent.gameObject + " : Unable To Lower Level Cap Bellow 1");
            MaxLevelCap = 1;
        }
        else
        {
            MaxLevelCap += ModifyAmount;
        }
    }

    public void ModifyCharacterLevel(int ModifyAmount)
    {
        if (CurrentLevel + ModifyAmount < 1 && MaxLevelCap != 0)
        {
            Debug.LogWarning(transform.parent.gameObject + " : Unable To Lower Current Level Below 1");
            CurrentLevel = 1;
        }
        else if (CurrentLevel + ModifyAmount > MaxLevelCap && MaxLevelCap != 0)
        {
            Debug.LogWarning(transform.parent.gameObject + " : Unable To Level Character Above Max Level Cap of " + MaxLevelCap);
            CurrentLevel = MaxLevelCap;
        }
        else
        {
            CurrentLevel += ModifyAmount;
        }
    }

    public UnityEvent GetCharacterLevelChangedEvent() => CharacterLevelChangeEvent;
}