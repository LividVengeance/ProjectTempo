using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    [SerializeField] private Image OffBeatFlash;
    [SerializeField] private Image OnBeatFlash;
    
    private readonly Color OnBeatFlashColour = new Vector4(1, 1, 1, .05f);
    private readonly Color OffBeatFlashColour = new Vector4(1, 0, 0, .5f);
    private readonly Color NoFlashColour = new Vector4(1, 1, 1, 0);
    private Coroutine OnBeatFlashCoroutine = null;
    private Coroutine OffBeatFlashCoroutine = null;
    [SerializeField] private float FlashTime = 0.1f;

    private void Start()
    {
        TempoManager.Instance.GetBeatUnityEvent().AddListener(OnBeat);
        TempoManager.Instance.GetActionOffBeatUnityEvent().AddListener(OffBeat);
    }

    private void OnBeat()
    {
        OnBeatFlashCoroutine = StartCoroutine(FlashActionColour(OnBeatFlashColour, OnBeatFlash));
    }

    private void OffBeat()
    {
        OffBeatFlashCoroutine = StartCoroutine(FlashActionColour(OffBeatFlashColour, OffBeatFlash));
    }

    private IEnumerator FlashActionColour(Color FlashColour, Image FlashImage)
    {
        FlashImage.color = FlashColour;
        yield return (new WaitForSeconds(FlashTime));
        FlashImage.color = NoFlashColour;
    }
}
