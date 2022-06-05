using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAction : GameplayAction
{
    private float DashDistance = .2f;
    
    
    public DashAction(ActionSystemComponent ActionSystemComponent, GameObject OwningGameObject) 
        : base(ActionSystemComponent, OwningGameObject) { }
    
    public override void StartAction()
    {
        base.StartAction();
        SetWillActionUpdate(false);
    }

    public override void PhysicsUpdateAction()
    {
        Vector3 MoveDirection = GetOwningCharacter().GetMoveDirection();
        
        Vector3 DashPosition = GetOwningCharacter().transform.position + MoveDirection * DashDistance;
        RaycastHit2D RaycastHit = Physics2D.Raycast(GetOwningCharacter().transform.position, MoveDirection, DashDistance);
        //if (RaycastHit.collider != null && RaycastHit.transform.gameObject.tag != "Player");
        //{
        //    DashPosition = RaycastHit.point;
        //}
        GetOwningCharacter().GetHeroRigidbody2D().MovePosition(DashPosition);
        ActionSystem.ChangeAction(null);
    }
}
