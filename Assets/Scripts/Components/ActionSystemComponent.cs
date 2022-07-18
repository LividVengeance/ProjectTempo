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

    public bool IsCurrentAction(GameplayAction InAction)
    {
        return CurrentGameplayAction == InAction;
    }

    // Returns the gameobject of the character that owns this action system
    public GameObject GetOwningCharacter() => OwningCharacter;
    /// Retruns the current action in this action system
    public GameplayAction GetCurrentAction() => CurrentGameplayAction;
    /// Ends the current action
    public void CancelCurrentAction() => CurrentGameplayAction.EndAction();
}
