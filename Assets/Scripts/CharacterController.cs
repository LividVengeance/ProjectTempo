using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.XR;
using Vector3 = UnityEngine.Vector3;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private InventoryComponent InventoryComponent;
    [SerializeField] private ActionSystemComponent ActionSystemComponent;
    [SerializeField] private VitalAttributesComponent VitalAttributesComponent;
    
    [SerializeField] private float MoveSpeed = 10f;

    private Rigidbody2D Rigidbody2D;
    private Vector3 MoveDirection;

    
    private void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        
        if (!InventoryComponent) Debug.LogError("No Inventory Component Has Been Assigned To " + gameObject.name);
        if (!ActionSystemComponent) Debug.LogError("No Action System Component Has Been Assigned To " + gameObject.name);
    }

    void Update()
    {
        float MoveX = 0f;
        float MoveY = 0f;
        
        if (Input.GetKey(KeyCode.W))
        {
            MoveY += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            MoveY -= 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            MoveX -= 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            MoveX += 1f;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (TempoManager.Instance.HasHitToTempo())
            {
                ActionSystemComponent.ChangeAction(ActionSystemComponent.DashActionState);

            }
            else TempoManager.Instance.GetActionOffBeatUnityEvent().Invoke();
        }

        if (Input.GetMouseButtonDown(0)) 
        {
            if (TempoManager.Instance.HasHitToTempo()) ActionSystemComponent.ChangeAction(ActionSystemComponent.MeleeAttackActionState);
            else TempoManager.Instance.GetActionOffBeatUnityEvent().Invoke();
        }

        MoveDirection = new Vector3(MoveX, MoveY).normalized;
    }

    private void FixedUpdate()
    {
        Rigidbody2D.velocity = MoveDirection * MoveSpeed;
    }

    public Vector3 GetMoveDirection() => MoveDirection;
    public Rigidbody2D GetHeroRigidbody2D() => Rigidbody2D;
    public InventoryComponent GetHeroInventoryComponent() => InventoryComponent;
}
