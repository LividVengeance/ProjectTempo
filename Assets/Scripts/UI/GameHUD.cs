using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    [Header("Inventory UI")]
    [SerializeField] private GameObject InventoryUIGameobject;
    [SerializeField] private GameObject InventoryUIConentGameobject;
    
    [Header("Flash UI ")]
    [SerializeField] private Image OffBeatFlash;
    [SerializeField] private Image OnBeatFlash;
    [SerializeField] private float FlashTime = 0.1f;
    [SerializeField] private bool bEnableFlash = true;

    // Flash
    private readonly Color OnBeatFlashColour = new Vector4(1, 1, 1, .05f);
    private readonly Color OffBeatFlashColour = new Vector4(1, 0, 0, .5f);
    private readonly Color NoFlashColour = new Vector4(1, 1, 1, 0);
    private Coroutine OnBeatFlashCoroutine = null;
    private Coroutine OffBeatFlashCoroutine = null;


    private void Start()
    {
        TempoManager.Instance.GetBeatUnityEvent().AddListener(OnBeat);
        TempoManager.Instance.GetActionOffBeatUnityEvent().AddListener(OffBeat);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (InventoryUIGameobject.activeSelf)
            {
                InventoryUIGameobject.GetComponent<InventoryUI>().CloseInventory();
                InventoryUIGameobject.SetActive(false);
            }
            else
            {
                InventoryUIGameobject.GetComponent<InventoryUI>().OpenInventory();
                InventoryUIGameobject.SetActive(true);
            }
        }
    }

    private void OnBeat()
    {
        if (bEnableFlash) OnBeatFlashCoroutine = StartCoroutine(FlashActionColour(OnBeatFlashColour, OnBeatFlash));
    }

    private void OffBeat()
    {
        if (bEnableFlash) OffBeatFlashCoroutine = StartCoroutine(FlashActionColour(OffBeatFlashColour, OffBeatFlash));
    }

    private IEnumerator FlashActionColour(Color FlashColour, Image FlashImage)
    {
        FlashImage.color = FlashColour;
        yield return (new WaitForSeconds(FlashTime));
        FlashImage.color = NoFlashColour;
    }

    public GameObject GetInventoryUIContent() => InventoryUIConentGameobject;
    public GameObject GetInventoryUIGameObject() => InventoryUIGameobject;
}
