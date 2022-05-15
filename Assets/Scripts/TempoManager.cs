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
    
    [SerializeField, Tooltip("Number of beats per minute")] 
    private int Tempo = 8;
    [SerializeField, Tooltip("Amount of time the player can hit to the beat earlier")] 
    private float PreTempLeniency = 0.5f;
    [SerializeField, Tooltip("Amount of time the player can hit to the beat late")] 
    private float PostTempLeniency = 0.5f;
    
    private AudioSource BeatAudioSource;
    
    // Events
    private UnityEvent BeatUnityEvent;
    private UnityEvent ActionOffBeat;

    private float CurrentTime = 0.0f;
    private float TimeBetweenBeats = 0.0f;
    
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        HeroCharacter = GameObject.FindWithTag("Player").GetComponent<CharacterController>();
        GameHud = GameObject.FindWithTag("GameHUD").GetComponent<GameHUD>();
        if (!GameHud) Debug.LogError("Unable To Find GameHUD In Scene");
        BeatAudioSource = GetComponent<AudioSource>();
        TimeBetweenBeats = 60.0f / Tempo;
        BeatUnityEvent = new UnityEvent();
        ActionOffBeat = new UnityEvent();
    }
    
    void Update()
    {
        CurrentTime += Time.deltaTime;
        if (CurrentTime >= TimeBetweenBeats)
        {
            BeatAudioSource.Play();
            BeatUnityEvent.Invoke();
            CurrentTime = 0.0f;
        }
    }
    
    public bool HasHitToTempo() => CurrentTime <= PostTempLeniency || CurrentTime >= TimeBetweenBeats + PreTempLeniency;
    public UnityEvent GetBeatUnityEvent() => BeatUnityEvent;
    public UnityEvent GetActionOffBeatUnityEvent() => ActionOffBeat;
    public GameHUD GetGameHUD() => GameHud;
    public CharacterController GetHeroCharacter() => HeroCharacter;
}
