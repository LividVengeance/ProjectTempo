using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MenuScreen
{
    [Header("Screen Objects")]
    [SerializeField] private GameObject PauseScreen;
    [SerializeField] private GameObject CheatScreen;

    [Header("Button Objects")]
    [SerializeField] private Button ReturnBttn;
    [SerializeField] private Button ExitGameBttn;
    [SerializeField] private Button CheatMenuBttn;

    private TempoManager TempoManager;
    private InputManager InputManager;
    private CharacterController HeroCharacter;
    private MenuManager MenuManager;


    private void OnEnable()
    {
        TempoManager = TempoManager.Instance;
        InputManager = TempoManager.GetInputManager();
        HeroCharacter = TempoManager.GetHeroCharacter();
        MenuManager = TempoManager.GetMenuManager();

        ClosePauseMenu();

        ReturnBttn.onClick.AddListener(ClosePauseMenu);
        ExitGameBttn.onClick.AddListener(OnExitGamePressed);
        CheatMenuBttn.onClick.AddListener(OnCheatMenuPressed);
    }

    private void Update()
    {
        // Back
        if (InputManager.GetMenuCancelDownState())
        {
            if (PauseScreen.activeSelf)
            {
                ClosePauseMenu();
            }
            else if (CheatScreen.activeSelf)
            {
                OpenPauseMenu();
            }
        }
    }

    private void OnCheatMenuPressed()
    {
        PauseScreen.SetActive(false);
        CheatScreen.SetActive(true);
    }

    private void OnExitGamePressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ClosePauseMenu()
    {
        PauseScreen.SetActive(false);
        CheatScreen.SetActive(false);
        FTrasnistionSettings Settings = InstantTransitionSettings;
        Settings.Screen = GetGameHUD();
        MenuManager.StartTransitionToScreen(Settings);

        TempoManager.DeincremnetPauseStack();

        //TODO: This is goning to cause an issue if the map was in the menu map before opening pause menu 
        InputManager.SwitchToGameMap();

        HeroCharacter.DeincrementDisableHeroMovementStack();
    }

    public void OpenPauseMenu()
    {
        gameObject.SetActive(true);
        MenuManager.StartTransitionToScreen(InstantTransitionSettings);

        TempoManager.IncrimentPauseStack();

        PauseScreen.SetActive(true);
        CheatScreen.SetActive(false);

        //TODO: This is goning to cause an issue if the map was in the menu map before opening pause menu 
        InputManager.SwitchToMenuMap();

        HeroCharacter.IncrementDisableHeroMovementStack();
    }
}
