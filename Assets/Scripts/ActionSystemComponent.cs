using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSystemComponent : MonoBehaviour
{
    [SerializeField] private GameplayAction CurrentGameplayAction;

    public DashAction DashActionState;
    public MeleeAttackAction MeleeAttackActionState;

    void Awake()
    {
        InitializeGameplayActions();
        CurrentGameplayAction = null;
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
        DashActionState = new DashAction(this, this.transform.gameObject);
        MeleeAttackActionState = new MeleeAttackAction(this, transform.gameObject);
    }
}
