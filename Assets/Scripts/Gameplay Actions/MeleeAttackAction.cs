using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackAction : AttackAction
{
    public MeleeAttackAction(ActionSystemComponent ActionSystemComponent, GameObject OwningGameObject) 
        : base(ActionSystemComponent, OwningGameObject) { }

    public override void StartAction()
    {
        base.StartAction();
        Collider2D = ColliderType == EColliderType.EBoxCollider ? new BoxCollider2D() : new CircleCollider2D();

        //Vector3 MousePositon = GetMouseWorldPosition(Input.mousePosition, Camera.main);
        //Vector3 AttackDirection = (MousePositon - GetOwningCharacter().transform.position);
        //
        //if (bDebugGameplayAction)
        //{
        //    Debug.Log(AttackDirection);
        //    Debug.DrawRay(GetOwningCharacter().transform.position, AttackDirection, Color.magenta, 3.0f);
        //}
    }

    public void OpenCollisionDetection()
    {

    }


}
