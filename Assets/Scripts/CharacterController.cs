using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.XR;
using Vector3 = UnityEngine.Vector3;

public class TempoCharacterController : MonoBehaviour
{
    protected InputManager InputManager;

    [Header("Controller Settings")]
    [SerializeField] private float MoveSpeed = 10f;
    [SerializeField] private int SpeedModifer = 1;

    protected Rigidbody2D Rigidbody2D;
    protected Vector3 MoveDirection;
    protected TempoCharacter OwningCharacter;
    
    
    private void Start()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        InputManager = TempoManager.Instance.GetInputManager();

        OwningCharacter = GetComponent<TempoCharacter>();
        if (!OwningCharacter) Debug.LogError("Character: " + this + " must have an owning TempoCharacter");
    }

    private void FixedUpdate()
    {
        float MovementSpeed = MoveSpeed * SpeedModifer;
        Rigidbody2D.velocity = MoveDirection * MovementSpeed;
    }

    public Vector3 GetMoveDirection() => MoveDirection;
    public Rigidbody2D GetHeroRigidbody2D() => Rigidbody2D;

    public int GetMovementSpeedModifier() => SpeedModifer;
    public void SetMovementSpeedModifier(int InSpeed) => SpeedModifer = InSpeed;
    public TempoCharacter GetOwningCharacter() => OwningCharacter;
}