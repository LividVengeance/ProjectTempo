using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TempManager : MonoBehaviour
{
    [SerializeField, Tooltip("Number of beats per minute")] 
    private int Tempo = 8;
    [SerializeField, Tooltip("Amount of time the player can hit to the beat earlier")] 
    private float PreTempLeniency = 0.5f;
    [SerializeField, Tooltip("Amount of time the player can hit to the beat late")] 
    private float PostTempLeniency = 0.5f;
    
    private AudioSource BeatAudioSource;

    private float CurrentTime = 0.0f;
    private float TimeBetweenBeats = 0.0f;
    
    
    void Start()
    {
        BeatAudioSource = GetComponent<AudioSource>();
        TimeBetweenBeats = 60.0f / Tempo;
    }
    
    void Update()
    {
        CurrentTime += Time.deltaTime;
        if (CurrentTime >= TimeBetweenBeats)
        {
            BeatAudioSource.Play();
            CurrentTime = 0.0f;
        }
    }

    public bool HasHitToTempo()
    {
        return CurrentTime <= PostTempLeniency || CurrentTime >= TimeBetweenBeats + PreTempLeniency;
    }
}
