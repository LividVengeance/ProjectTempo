using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackAction : AttackAction
{
    public MeleeAttackAction(ActionSystemComponent ActionSystemComponent, GameObject OwningGameObject) 
        : base(ActionSystemComponent, OwningGameObject) { }

    public override void StartAction()
    {
        Vector3 MousePositon = GetMouseWorldPosition(Input.mousePosition, Camera.main);
        Vector3 AttackDirection = (MousePositon - GetOwningCharacter().transform.position);
        Debug.Log(AttackDirection);
        Debug.DrawRay(GetOwningCharacter().transform.position, AttackDirection, Color.magenta, 3.0f);
    }
}
