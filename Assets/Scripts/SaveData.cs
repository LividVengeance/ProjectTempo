using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    /// Player Data
    // Vital Attributes
    public float SD_MaxPlayerHealth;
    public float SD_CurrentPlayerHealth;
    public int SD_PlayerCurrency;
    // Progression
    public int SD_CurrentPlayerLevel;
    public int SD_PlayerMaxLevel;
    public int SD_CurrentPlayerExperience;
    // Inventory

    public float[] SD_CurrentPlayerPosition;

    public SaveData(CharacterController PlayerCharacter)
    {
        // Player Position
        SD_CurrentPlayerPosition = new float[3];
        SD_CurrentPlayerPosition[0] = PlayerCharacter.gameObject.transform.position.x;
        SD_CurrentPlayerPosition[1] = PlayerCharacter.gameObject.transform.position.y;
        SD_CurrentPlayerPosition[2] = PlayerCharacter.gameObject.transform.position.z;

        // Vital Attributes
        VitalAttributesComponent VitalAttributes = PlayerCharacter.GetVitalAttributesComponent();
        SD_CurrentPlayerHealth = VitalAttributes.GetCharacterCurrentHealth();
        SD_MaxPlayerHealth = VitalAttributes.GetCharacterMaxHealth();

        // Progression
        ProgressionComponent Progression = PlayerCharacter.GetProgressionComponent();
        SD_CurrentPlayerLevel = Progression.GetCharacterCurrentLevel();
        SD_PlayerMaxLevel = Progression.GetCharacterMaxLevel();
        SD_CurrentPlayerExperience = Progression.GetCharacterCurrentExperience();
        SD_PlayerCurrency = Progression.GetCurrency();

        // Inventory
    }
}
