using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public class TempoManager : MonoBehaviour
{
    public static TempoManager Instance { get; private set; }

    private HUDScreen GameHud;
    private HeroCharacter HeroCharacter;
    private InputManager InputManager;
    private MenuManager MenuManager;

    private TempoGameUserSettings GameUserSettings; // This will likely need to moved later 
    private TempoGameSystemSettings GameSystemSettings;

    private int PauseStack = 0;

    void Awake()
    {
        // Set up Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        HeroCharacter = GameObject.FindWithTag("Player").GetComponent<HeroCharacter>();
        if (!HeroCharacter) Debug.LogError("Unable To Find Hero Character Controller");
        GameHud = GameObject.FindWithTag("GameHUD").GetComponent<HUDScreen>();
        if (!GameHud) Debug.LogError("Unable To Find GameHUD In Scene");
        InputManager = GameObject.FindWithTag("InputManager").GetComponent<InputManager>();
        if (!InputManager) Debug.LogError("Unable To Find Input Manager");
        MenuManager = GameObject.FindWithTag("MenuManager").GetComponent<MenuManager>();
        if (!MenuManager) Debug.LogError("Unable To Find Menu Manager");

        GameUserSettings = GetComponent<TempoGameUserSettings>();
        GameSystemSettings = GetComponent<TempoGameSystemSettings>();
    }

    public void IncrimentPauseStack()
    {
        PauseStack++;
        if (PauseStack > 0)
        {
            Time.timeScale = 0;
        }
    }

    public void DeincremnetPauseStack()
    {
        PauseStack--;
        if (PauseStack <= 0)
        {
            Time.timeScale = 1;
        }
    }
    
    public HUDScreen GetGameHUD() => GameHud;
    public HeroCharacter GetHeroCharacter() => HeroCharacter;
    public InputManager GetInputManager() => InputManager;
    public MenuManager GetMenuManager() => MenuManager;
    public TempoGameUserSettings GetGameUserSettings() => GameUserSettings;
    public TempoGameSystemSettings GetGameSystemSettings() => GameSystemSettings;
}
