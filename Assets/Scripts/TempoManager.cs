using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public class TempoManager : MonoBehaviour
{
    public static TempoManager Instance { get; private set; }

    private GameHUD GameHud;
    private CharacterController HeroCharacter;
    private InputManager InputManager;
    private MenuManager MenuManager;

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

        HeroCharacter = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
        if (!HeroCharacter) Debug.LogError("Unable To Find Hero Character Controller");
        GameHud = GameObject.FindWithTag("GameHUD").GetComponent<GameHUD>();
        if (!GameHud) Debug.LogError("Unable To Find GameHUD In Scene");
        InputManager = GameObject.FindWithTag("InputManager").GetComponent<InputManager>();
        if (!InputManager) Debug.LogError("Unable To Find Input Manager");
        MenuManager = GameObject.FindWithTag("MenuManager").GetComponent<MenuManager>();
        if (!MenuManager) Debug.LogError("Unable To Find Menu Manager");
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
    
    public GameHUD GetGameHUD() => GameHud;
    public CharacterController GetHeroCharacter() => HeroCharacter;
    public InputManager GetInputManager() => InputManager;
    public MenuManager GetMenuManager() => MenuManager;
}
