using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAction : GameplayAction
{
    public AttackAction(ActionSystemComponent ActionSystemComponent, GameObject OwningGameObject) 
        : base(ActionSystemComponent, OwningGameObject) { }

    public enum EColliderType
    {
        EBoxCollider,
        ECircleCollider,
    };

    public enum EAimType
    {
        ECursorPosition,
        ECursorDirection,
        EPlayerPosition,
        EPlayerDirection,
        EClosestEnemyPosition,
    };

    public EColliderType ColliderType;
    public EAimType AimType;
    public Collider2D Collider2D;

    public Vector3 GetAimColliderPosition()
    {
        Vector3 ColliderPosition = Vector3.zero;
        Vector3 MousePositon = GetMouseWorldPosition(Input.mousePosition, Camera.main);
        switch (AimType)
        {
            case EAimType.ECursorPosition:
                ColliderPosition = MousePositon;
                break;
            case EAimType.ECursorDirection:
                ColliderPosition = (MousePositon - GetOwningCharacter().transform.position);
                break;
            case EAimType.EPlayerPosition:
                ColliderPosition = TempoManager.Instance.GetHeroCharacter().transform.position;
                break;
            case EAimType.EClosestEnemyPosition:
                ColliderPosition = GetClosestTarget().transform.position;
                break;
            default:
                break;
        }

        return ColliderPosition;
    }

    public GameObject GetClosestTarget()
    {
        GameObject[] EnemyTargets = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject ClosestTarget = null;
        Vector3 PlayerCharacter = TempoManager.Instance.GetHeroCharacter().gameObject.transform.position;
        foreach(GameObject Target in EnemyTargets)
        {
            if (ClosestTarget == null || 
                Vector3.Distance(Target.transform.position, PlayerCharacter) < Vector3.Distance(ClosestTarget.transform.position, PlayerCharacter))
            {
                ClosestTarget = Target;
            }
        }
        return ClosestTarget;
    }
}
