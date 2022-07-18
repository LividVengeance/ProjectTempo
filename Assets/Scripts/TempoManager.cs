using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public class TempoManager : MonoBehaviour
{
    public static TempoManager Instance { get; private set;  }

    private GameHUD GameHud;
    private CharacterController HeroCharacter;
    private InputManager InputManager;
    
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
    }
    
    public GameHUD GetGameHUD() => GameHud;
    public CharacterController GetHeroCharacter() => HeroCharacter;
    public InputManager GetInputManager() => InputManager;
}
