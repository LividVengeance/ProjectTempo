using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class ProgressionComponent : MonoBehaviour
{
    // Events
    private UnityEvent CharacterLevelChangeEvent;
    private UnityEvent CharacterExperienceChangeEvent;

    [Header("Leveling")]
    [ShowInInspector, ReadOnly] 
    private int CurrentExperience;
    [SerializeField, Tooltip("The max amount of experience this character can hold. Keep 0 to have no cap")] 
    private int MaxExperienceCap;
    [ShowInInspector, ReadOnly] 
    private int CurrentLevel;
    [SerializeField, Tooltip("The maxium level this charater can be. Keep 0 to have no cap")] 
    private int MaxLevelCap;

    [Header("Currency")]
    [ShowInInspector, ReadOnly] private int Currency;
    [SerializeField, Tooltip("The maxium currency this character can hold. Keep 0 to have no cap")]
    private int MaxCurrencyCap;

    public bool ModifyCharacterExperience(int ModifyAmount)
    {
        bool bAbleToModifyFullAmount = false;
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
            Debug.Log(transform.parent.gameObject + " : Modified character experience by " + ModifyAmount);
            bAbleToModifyFullAmount = true;
        }
        CharacterExperienceChangeEvent.Invoke();
        return bAbleToModifyFullAmount;
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
            Debug.Log(transform.parent.gameObject + " : Modified character max level cap by " + ModifyAmount);
        }
    }

    public bool ModifyCharacterLevel(int ModifyAmount)
    {
        bool bAbleToModifyFullAmount = false;
        if (CurrentLevel + ModifyAmount < 1 && MaxLevelCap > 0)
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
            Debug.Log(transform.parent.gameObject + " : Modified character level by " + ModifyAmount);
            bAbleToModifyFullAmount = true;
        }
        CharacterLevelChangeEvent.Invoke();
        return bAbleToModifyFullAmount;
    }

    public bool ModifyCharacterCurrency(int ModifyAmount)
    {
        bool bAbleToModifyFullAmount = false;
        if (Currency + ModifyAmount > MaxCurrencyCap && MaxCurrencyCap > 0)
        {
            Currency = ModifyAmount;
            int GivenAmount = (Currency + ModifyAmount) - MaxCurrencyCap;
            Debug.LogWarning(transform.parent.gameObject + " : Unable to give full currency amount. Instead gave " + GivenAmount +  " of " + ModifyAmount);
        }
        else if (Currency - ModifyAmount < 0)
        {
            Currency = 0;
            Debug.LogWarning(transform.parent.gameObject + " : Unable to remove full currency amount. Instead removed " + Currency + " of " + ModifyAmount);
        }
        else
        {
            Currency += ModifyAmount;
            Debug.Log(transform.parent.gameObject + " : Modified character currency by " + ModifyAmount);
            bAbleToModifyFullAmount = true;
        }
        return bAbleToModifyFullAmount;
    }

    public void ModifyCharacterMaxCurrencyCap(int ModifyAmount)
    {
        if (MaxCurrencyCap + ModifyAmount < 0)
        {
            MaxCurrencyCap = 0;
            Debug.LogWarning(transform.parent.gameObject + " : Unable to lower currency cap below 0");
        }
        else
        {
            MaxCurrencyCap += ModifyAmount;
            Debug.Log(transform.parent.gameObject + " : Modify character max currency by " + ModifyAmount);
        }
    }

    public UnityEvent GetCharacterLevelChangedEvent() => CharacterLevelChangeEvent;
    public UnityEvent GetChracterExperenceChangedEvent() => CharacterExperienceChangeEvent;
    public int GetCharacterCurrentLevel() => CurrentLevel;
    public int GetCharacterMaxLevel() => MaxLevelCap;
    public int GetCharacterCurrentExperience() => CurrentExperience;

    public int GetCurrency() => Currency;
    public int GetMaxCurrencyCap() => MaxCurrencyCap;
}