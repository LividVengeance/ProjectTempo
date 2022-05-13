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
    [SerializeField] private float DashDistance = 10f;
    
    private bool bIsDashInputDown = false;
    
    private Rigidbody2D Rigidbody2D;
    private Vector3 MoveDirection;

    
    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
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
            bIsDashInputDown = true;
        }

        MoveDirection = new Vector3(MoveX, MoveY).normalized;
    }

    private void FixedUpdate()
    {
        Rigidbody2D.velocity = MoveDirection * MoveSpeed;

        if (bIsDashInputDown)
        {
            Vector3 DashPosition = transform.position + MoveDirection * DashDistance;
            RaycastHit2D RaycastHit = Physics2D.Raycast(transform.position, MoveDirection, DashDistance);
            //if (RaycastHit.collider != null && RaycastHit.transform.gameObject.tag != "Player");
            //{
            //    DashPosition = RaycastHit.point;
            //}
            Rigidbody2D.MovePosition(DashPosition);
            bIsDashInputDown = false;
        }
    }
}
