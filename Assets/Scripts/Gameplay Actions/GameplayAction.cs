using UnityEngine;

public class GameplayAction
{
    private bool bWillActionUpdate = true;
    private bool bWillActionPhysicsUpdate = true;
    protected bool bDebugGameplayAction = false;
    private GameObject OwningGameObject;
    private readonly CharacterController OwningCharacter;
    protected ActionSystemComponent ActionSystem;
    
    public GameplayAction(ActionSystemComponent ActionSystemComponent, GameObject OwningGameObject)
    {
        this.ActionSystem = ActionSystemComponent;
        this.OwningGameObject = OwningGameObject;
        this.OwningCharacter = OwningGameObject.GetComponent<CharacterController>();
    }
    
    public virtual void StartAction() { }

    public virtual void UpdateAction() { }

    public virtual void PhysicsUpdateAction() { }

    public virtual void EndAction() { }

    public bool WillActionUpdate() => bWillActionUpdate;
    public bool WillActionPhysicsUpdate() => bWillActionPhysicsUpdate;
    protected void SetWillActionUpdate(bool bWillUpdate) => bWillActionUpdate = bWillUpdate;
    protected void SetWillActionPhysicsUpdate(bool bWillUpdate) => bWillActionPhysicsUpdate = bWillUpdate;
    protected CharacterController GetOwningCharacter() => OwningCharacter;
    protected Vector3 GetMouseWorldPosition(Vector3 ScreenPosition, Camera WorldCamera) => WorldCamera.ScreenToWorldPoint(ScreenPosition);
}
