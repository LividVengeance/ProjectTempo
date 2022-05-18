using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ActionSystemComponent : MonoBehaviour
{
    [ReadOnly]
    private GameplayAction CurrentGameplayAction;
    private GameObject OwningCharacter;

    public DashAction DashActionState;
    public MeleeAttackAction MeleeAttackActionState;
    public StruggleAction StruggleActionState;
    public DeathAction DeathActionState;

    void Start()
    {
        OwningCharacter = transform.parent.gameObject;
        CurrentGameplayAction = null;
        
        InitializeGameplayActions();
    }

    void Update()
    {
        if (CurrentGameplayAction != null && CurrentGameplayAction.WillActionUpdate())
        {
            CurrentGameplayAction.UpdateAction();
        }
    }

    void FixedUpdate()
    {
        if (CurrentGameplayAction != null && CurrentGameplayAction.WillActionPhysicsUpdate())
        {
            CurrentGameplayAction.PhysicsUpdateAction();
        }
    }

    public void ChangeAction(GameplayAction NewAction)
    {
        CurrentGameplayAction?.EndAction();

        CurrentGameplayAction = NewAction;
        CurrentGameplayAction?.StartAction();
    }

    private void InitializeGameplayActions()
    {
        DashActionState = new DashAction(this, OwningCharacter);
        MeleeAttackActionState = new MeleeAttackAction(this, OwningCharacter);
        StruggleActionState = new StruggleAction(this, OwningCharacter);
        DeathActionState = new DeathAction(this, OwningCharacter);   
    }

    public GameObject GetOwningCharacter() => OwningCharacter;
}
