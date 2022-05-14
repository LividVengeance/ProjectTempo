using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.XR;
using Vector3 = UnityEngine.Vector3;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 10f;

    private Rigidbody2D Rigidbody2D;
    private Vector3 MoveDirection;

    private TempManager TempoManager;
    private ActionSystemComponent ActionSystem;

    
    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        ActionSystem = GetComponent<ActionSystemComponent>();
        TempoManager = GameObject.FindWithTag("TempoManager").GetComponent<TempManager>();
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
            if (TempoManager.HasHitToTempo())
            {
                ActionSystem.ChangeAction(ActionSystem.DashActionState);
            }
        }

        MoveDirection = new Vector3(MoveX, MoveY).normalized;
    }

    private void FixedUpdate()
    {
        Rigidbody2D.velocity = MoveDirection * MoveSpeed;
    }

    public Vector3 GetMoveDirection() => MoveDirection;
    public Rigidbody2D GetHeroRigidbody2D() => Rigidbody2D;
}
