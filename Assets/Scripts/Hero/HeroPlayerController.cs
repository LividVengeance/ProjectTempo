using System.Collections;
using System.Collections.Generic;

using UnityEngine.InputSystem;
using UnityEngine;

public class HeroPlayerController : TempoCharacterController
{
    MenuManager MenuManager;
    private int DisableHeroMovementStack = 0;

    private void Awake()
    {
        MenuManager = TempoManager.Instance.GetMenuManager();
        if (!MenuManager) Debug.LogError("HeroPlayerController was unable to get menu manager");
    }

    public void OnInputActionRecieved(InputAction.CallbackContext InContext)
    {
        UpdateControllerMovement(InContext);
        HandleHeroInputActions(InContext.action.name, InContext.phase);
    }

    private void UpdateControllerMovement(InputAction.CallbackContext InContext)
    {
        float MoveX = 0f;
        float MoveY = 0f;

        if (DisableHeroMovementStack <= 0)
        {
            if (InputManager.GetMovementInputState().y > 0)
            {
                MoveY += 1f;
            }
            if (InputManager.GetMovementInputState().y < 0)
            {
                MoveY -= 1f;
            }
            if (InputManager.GetMovementInputState().x < 0)
            {
                MoveX -= 1f;
            }
            if (InputManager.GetMovementInputState().x > 0)
            {
                MoveX += 1f;
            }
        }

        MoveDirection = new Vector3(MoveX, MoveY).normalized;
    }

    private void HandleHeroInputActions(string InActionName, InputActionPhase InActionPhase)
    {
        switch (InActionPhase)
        {
            case InputActionPhase.Started:
            {
                OnInputActionDown(InActionName);
                break;
            }
            case InputActionPhase.Canceled:
            {
                OnInputActionUp(InActionName);
                break;
            }
            default:
            {
                break;
            }
        }
    }

    private bool OnInputActionDown(string InActionName)
    {
        bool bHandledInput = false;

        if (InActionName == "Save")
        {
            SavePlayerData();
            bHandledInput = true;
        }
        if (InActionName == "LoadSave")
        {
            LoadPlayerData();
            bHandledInput = true;
        }
        return bHandledInput;
    }

    private bool OnInputActionUp(string InActionName)
    {
        bool bHandledInput = false;
        return bHandledInput;
    }

    private void SavePlayerData()
    {
        SaveSystem.SaveAllPlayerData((HeroCharacter)GetOwningCharacter());
    }

    private void LoadPlayerData()
    {
        SaveData Data = SaveSystem.LoadData();
    }

    public void IncrementDisableHeroMovementStack()
    {
        DisableHeroMovementStack++;
        MoveDirection = Vector3.zero;
    }

    public void DeincrementDisableHeroMovementStack()
    {
        DisableHeroMovementStack--;
    }

    public bool GetHeroDisabledMovementState() => DisableHeroMovementStack > 0;
}
