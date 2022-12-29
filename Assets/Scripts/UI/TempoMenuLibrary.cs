using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempoMenuLibrary : MonoBehaviour
{
    private TempoManager TempoManager;
    private InputManager InputManager;

    Dictionary<string, MenuScreen> TempoMenuScreens = new Dictionary<string, MenuScreen>();

    [SerializeField] private string PauseScreenName = "PauseMenu";
    [Header("Debug")]
    [SerializeField] private bool bDisplayDebugInfo = false;

    void Start()
    {
        TempoManager = TempoManager.Instance;
        InputManager = TempoManager.GetInputManager();
    
        for (int Index = transform.childCount -1; Index >= 0; Index--)
        {
            Transform Child = transform.GetChild(Index);
            MenuScreen Screen;
            if (Child.TryGetComponent<MenuScreen>(out Screen))
            {
                TempoMenuScreens.Add(Child.name, Screen);
            }
        }
    }

    void Update()
    {
        PauaseMenuScreenInputHandler();
    }

    void PauaseMenuScreenInputHandler()
    {
        // Open pause menu
        if (InputManager.GetGamePauseDownInputState())
        {
            MenuScreen PauseScreen;
            if (TempoMenuScreens.TryGetValue(PauseScreenName, out PauseScreen))
            {
                PauseMenu PauseMenuScreen = (PauseMenu)PauseScreen;
                PauseMenuScreen.OpenPauseMenu();
                if (bDisplayDebugInfo) Debug.Log("MenuLibrary: Open Pause Menu");
            }

        }
        // Close menu
        else if (InputManager.GetMenuUnpauseDownInputState())
        {
            MenuScreen PauseScreen;
            if (TempoMenuScreens.TryGetValue(PauseScreenName, out PauseScreen))
            {
                PauseMenu PauseMenuScreen = (PauseMenu)PauseScreen;
                PauseMenuScreen.ClosePauseMenu();
                if (bDisplayDebugInfo) Debug.Log("MenuLibrary: Close Pause Menu");
            }
        }
    }

    public VirtualCursor GetVirtualCursor() => gameObject.GetComponent<VirtualCursor>();
}
