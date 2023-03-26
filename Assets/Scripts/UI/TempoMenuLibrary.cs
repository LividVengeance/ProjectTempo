using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempoMenuLibrary : MonoBehaviour
{
    private TempoManager TempoManager;
    private InputManager InputManager;

    Dictionary<string, MenuScreen> TempoMenuScreens = new Dictionary<string, MenuScreen>();

    [SerializeField] public string PauseScreenName = "PauseMenu";
    [SerializeField] public string InventoryScreenName = "InventoryScreen";
    [Header("Debug")]
    [SerializeField] private bool bDisplayDebugInfo = false;

    void Awake()
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

    public VirtualCursor GetVirtualCursor() => gameObject.GetComponent<VirtualCursor>();
    public Dictionary<string, MenuScreen> GetMenuScreens() => TempoMenuScreens;
}
