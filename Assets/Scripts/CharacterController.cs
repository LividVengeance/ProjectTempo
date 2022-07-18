using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.XR;
using Vector3 = UnityEngine.Vector3;

public class CharacterController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private InventoryComponent InventoryComponent;
    [SerializeField] private ActionSystemComponent ActionSystemComponent;
    [SerializeField] private VitalAttributesComponent VitalAttributesComponent;
    [SerializeField] private ProgressionComponent ProgressionComponent;
    private InputManager InputManager;

    [Header("Controller Settings")]
    [SerializeField] private float MoveSpeed = 10f;

    private Rigidbody2D Rigidbody2D;
    private Vector3 MoveDirection;

    
    private void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        InputManager = TempoManager.Instance.GetInputManager();
        
        if (!InventoryComponent) Debug.LogError("No Inventory Component Has Been Assigned To " + gameObject.name);
        if (!ActionSystemComponent) Debug.LogError("No Action System Component Has Been Assigned To " + gameObject.name);
    }

    void Update()
    {
        InputUpdate();
    }

    private void FixedUpdate()
    {
        Rigidbody2D.velocity = MoveDirection * MoveSpeed;
    }

    private void InputUpdate()
    {
        float MoveX = 0f;
        float MoveY = 0f;

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
        if (InputManager.GetSaveDownInputState())
        {
            SavePlayerData();
        }
        if (InputManager.GetLoadSaveDownState())
        {
            LoadPlayerData();
        }

        if (InputManager.GetActionThreeDownInputState())
        {
            ActionSystemComponent.ChangeAction(ActionSystemComponent.DashActionState);
        }

        if (InputManager.GetActionOneDownInputState())
        {
            ActionSystemComponent.ChangeAction(ActionSystemComponent.MeleeAttackActionState);
        }

        MoveDirection = new Vector3(MoveX, MoveY).normalized;
    }

    private void SavePlayerData()
    {
        SaveSystem.SaveAllPlayerData(this);
    }

    private void LoadPlayerData()
    {
        SaveData Data = SaveSystem.LoadData();
    }

    public Vector3 GetMoveDirection() => MoveDirection;
    public Rigidbody2D GetHeroRigidbody2D() => Rigidbody2D;
    public InventoryComponent GetHeroInventoryComponent() => InventoryComponent;
    public VitalAttributesComponent GetVitalAttributesComponent() => VitalAttributesComponent;
    public ProgressionComponent GetProgressionComponent() => ProgressionComponent;
}
