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

    [SerializeField] FTrasnistionSettings TransitionSetings;

    private InputManager InputManager;
    private CharacterController HeroCharacter;
    private MenuManager MenuManager;

    private bool bPreInputState = false;

    private void Start()
    {
        InputManager = TempoManager.Instance.GetInputManager();
        HeroCharacter = TempoManager.Instance.GetHeroCharacter();
        MenuManager = TempoManager.Instance.GetMenuManager();

        ClosePauseMenu();

        ReturnBttn.onClick.AddListener(ClosePauseMenu);
        ExitGameBttn.onClick.AddListener(OnExitGamePressed);
        CheatMenuBttn.onClick.AddListener(OnCheatMenuPressed);
    }

    private void Update()
    {
        // Open pause menu
        if (InputManager.GetGamePauseDownInputState())
        {
            bPreInputState = HeroCharacter.GetHeroDisabledMovementState();
            OpenPauseMenu();
            
        }
        // Close menu
        else if (InputManager.GetMenuUnpauseDownInputState())
        {
            ClosePauseMenu();
        }
        // Back
        else if (InputManager.GetMenuCancelDownState())
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

        // Ensure the cursor is enabled
        InputManager.EnableCursor();
    }

    private void OnExitGamePressed()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void ClosePauseMenu()
    {
        PauseScreen.SetActive(false);
        CheatScreen.SetActive(false);

        //TODO: This is goning to cause an issue if the map was in the menu map before opening pause menu 
        InputManager.SwitchToGameMap();
        InputManager.DisableCursor();

        HeroCharacter.DisableHeroMovement(bPreInputState);
    }

    private void OpenPauseMenu()
    {
        TransitionSetings.Delay = 0.0f;
        TransitionSetings.TransitionTime = 0.15f;
        TransitionSetings.TransitionType = ETransitionType.Instant;
        TransitionSetings.Screen = this;
        TransitionSetings.HoldFadeTime = 0.0f;
        MenuManager.StartTransitionToScreen(TransitionSetings);

        PauseScreen.SetActive(true);
        CheatScreen.SetActive(false);

        //TODO: This is goning to cause an issue if the map was in the menu map before opening pause menu 
        InputManager.SwitchToMenuMap();

        HeroCharacter.DisableHeroMovement(true);
    }
}
