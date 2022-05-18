using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAction : GameplayAction
{
    public DeathAction(ActionSystemComponent ActionSystemComponent, GameObject OwningGameObject)
        : base(ActionSystemComponent, OwningGameObject) { }

    public override void StartAction()
    {
        base.StartAction();
    }
}
