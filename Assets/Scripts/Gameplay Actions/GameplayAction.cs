using UnityEngine;

public class GameplayAction
{
    private bool bWillActionUpdate = true;
    private bool bWillActionPhysicsUpdate = true;
    private GameObject OwningCharacter;
    protected ActionSystemComponent ActionSystem;
    
    public GameplayAction(ActionSystemComponent ActionSystemComponent, GameObject OwningGameObject)
    {
        this.ActionSystem = ActionSystemComponent;
        this.OwningCharacter = OwningGameObject;
    }
    
    public virtual void StartAction() { }

    public virtual void UpdateAction() { }

    public virtual void PhysicsUpdateAction() { }

    public virtual void EndAction() { }

    public void SetWillActionUpdate(bool bWillUpdate) => bWillActionUpdate = bWillUpdate;
    public bool WillActionUpdate() => bWillActionUpdate;
    public void SetWillActionPhysicsUpdate(bool bWillUpdate) => bWillActionPhysicsUpdate = bWillUpdate;
    public bool WillActionPhysicsUpdate() => bWillActionPhysicsUpdate;
    public CharacterController GetOwningCharacter() => OwningCharacter.GetComponent<CharacterController>();

}
