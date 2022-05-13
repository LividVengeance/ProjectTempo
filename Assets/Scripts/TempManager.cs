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
    
    private UnityEvent TempoUnityEvent;

    private float CurrentTime = 0.0f;
    private float TimeBetweenBeats = 0.0f;
    
    
    void Start()
    {
        TimeBetweenBeats = Tempo / 60.0f;
    }
    
    void Update()
    {
        CurrentTime += Time.deltaTime;
        if (CurrentTime >= TimeBetweenBeats)
        {
            CurrentTime = 0.0f;
            TempoUnityEvent.Invoke();
        }
    }

    public bool HasHitToTempo()
    {
        return (CurrentTime + PreTempLeniency >= TimeBetweenBeats 
                || ((CurrentTime - PreTempLeniency <= TimeBetweenBeats) 
                    && (CurrentTime - PreTempLeniency >= CurrentTime + PreTempLeniency)));
    }
}
