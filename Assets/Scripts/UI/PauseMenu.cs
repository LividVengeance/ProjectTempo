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
    private HeroCharacter HeroCharacter;
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

    public override bool OnActionDown(string InActionName)
    {
        bool bHandledInput = false;
        if (InActionName == "OpenCloseInventory")
        {
            if (PauseScreen.activeSelf)
            {
                ClosePauseMenu();
                bHandledInput = true;
            }
            else if (CheatScreen.activeSelf)
            {
                OpenPauseMenu();
                bHandledInput = true;
            }
        }
        else if (InActionName == "Pause" || InActionName == "Unpause")
        {
            if (PauseScreen.activeSelf)
            {
                ClosePauseMenu();
            }
            else
            {
                OpenPauseMenu();
            }
            bHandledInput = true;
        }
        return bHandledInput;
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
        MenuManager.StartTransitionToHUDScreen();

        TempoManager.DeincremnetPauseStack();

        InputManager.SwitchToGameMap();

        HeroCharacter.GetHeroController().DeincrementDisableHeroMovementStack();
    }

    public void OpenPauseMenu()
    {
        gameObject.SetActive(true);
        MenuManager.StartTransitionToScreen(InstantTransitionSettings);

        TempoManager.IncrimentPauseStack();

        PauseScreen.SetActive(true);
        CheatScreen.SetActive(false);

        InputManager.SwitchToMenuMap();

        HeroCharacter.GetHeroController().IncrementDisableHeroMovementStack();
    }

    public bool IsPauseScreenOpen() => gameObject.activeSelf;
}
