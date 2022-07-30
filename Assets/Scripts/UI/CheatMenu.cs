using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CheatMenu : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private Button ReturnBttn;

    [Header("Cheat Toggles")]
    [SerializeField] private Toggle GodModeToggle;
    [SerializeField] private Toggle TitanModeToggle;
    [SerializeField] private Toggle OneHitKillToggle;
    [SerializeField] private TMP_Dropdown SpeedModifierDropDown;

    private CharacterController HeroCharacter;


    private void Start()
    {
        HeroCharacter = TempoManager.Instance.GetHeroCharacter();

        GodModeToggle.isOn = HeroCharacter.GetVitalAttributesComponent().GetGodModeState();
        TitanModeToggle.isOn = HeroCharacter.GetVitalAttributesComponent().GetTitanModeState();
        //TODO: Set toggle state for oneHitKill
    }

    private void OnEnable()
    {
        GodModeToggle.onValueChanged.AddListener(delegate
        {
            OnGodModeToggleChanged(GodModeToggle);
        });

        TitanModeToggle.onValueChanged.AddListener(delegate
        {
            OnTitanModeToggleChanged(TitanModeToggle);
        });

        OneHitKillToggle.onValueChanged.AddListener(delegate
        {
            OnOneHitKillToggleChanaged(OneHitKillToggle);
        });

        SpeedModifierDropDown.onValueChanged.AddListener(delegate
        {
            OnSpeedModifierDropDownChanged(SpeedModifierDropDown);
        });

        ReturnBttn.onClick.AddListener(OnReturnBttnPress);
    }

    private void OnDisable()
    {
        GodModeToggle.onValueChanged.RemoveAllListeners();
        ReturnBttn.onClick.RemoveListener(OnReturnBttnPress);
    }

    private void OnReturnBttnPress()
    {
        PauseMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    private void OnGodModeToggleChanged(Toggle InToggle)
    {
        HeroCharacter.GetVitalAttributesComponent().SetGodModeState(InToggle.isOn);
    }

    private void OnTitanModeToggleChanged(Toggle InToggle)
    {
        HeroCharacter.GetVitalAttributesComponent().SetTitanModeState(InToggle.isOn);
    }

    private void OnOneHitKillToggleChanaged(Toggle InToggle)
    {
        //TODO: Implement one hit kill modifier
    }

    private void OnSpeedModifierDropDownChanged(TMP_Dropdown InDropDown)
    {
        Debug.Log("Dropdown changed");
        HeroCharacter.SetMovementSpeedModifier((int)InDropDown.value);
    }
}
