using UnityEngine;

public abstract class GameplayAction
{
    private bool bWillActionUpdate = true;
    private bool bWillActionPhysicsUpdate = true;
    protected bool bDebugGameplayAction = false;
    private GameObject OwningGameObject;
    private readonly TempoCharacterController OwningCharacter;
    protected ActionSystemComponent ActionSystem;

    public delegate void GameplayActionDelegate(GameplayAction Action);
    private GameplayActionDelegate OnActionStarted;
    private GameplayActionDelegate OnActionEnded;
    
    public GameplayAction(ActionSystemComponent ActionSystemComponent, GameObject OwningGameObject)
    {
        this.ActionSystem = ActionSystemComponent;
        this.OwningGameObject = OwningGameObject;
        this.OwningCharacter = OwningGameObject.GetComponent<TempoCharacterController>();
    }
    
    public virtual void StartAction() 
    {
        if (OnActionStarted != null)
        {
            OnActionStarted.Invoke(this);
        }
    }

    public virtual void UpdateAction() { }

    public virtual void PhysicsUpdateAction() { }

    public virtual void EndAction() 
    {
        if (OnActionEnded != null)
        {
            OnActionEnded.Invoke(this); 
        }
    }

    public bool WillActionUpdate() => bWillActionUpdate;
    public bool WillActionPhysicsUpdate() => bWillActionPhysicsUpdate;
    protected void SetWillActionUpdate(bool bWillUpdate) => bWillActionUpdate = bWillUpdate;
    protected void SetWillActionPhysicsUpdate(bool bWillUpdate) => bWillActionPhysicsUpdate = bWillUpdate;
    protected TempoCharacterController GetOwningCharacter() => OwningCharacter;
    protected Vector3 GetMouseWorldPosition(Vector3 ScreenPosition, Camera WorldCamera) => WorldCamera.ScreenToWorldPoint(ScreenPosition);

    public GameplayActionDelegate GetActionStartedDelegate() => OnActionStarted;
    public GameplayActionDelegate GetActionEndedDelegate() => OnActionEnded;
}