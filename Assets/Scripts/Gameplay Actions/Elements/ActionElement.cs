using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionElement : MonoBehaviour
{
    public bool bAutoActivate = false;
    private GameplayAction OwningAction;

    GameplayAction.GameplayActionDelegate StartedDelegate;
    GameplayAction.GameplayActionDelegate EndedDelegate;

    public ActionElement(GameplayAction InAction, bool bInAutoActivate)
    {
        OwningAction = InAction;    
        bAutoActivate = bInAutoActivate;
        if (bAutoActivate && InAction != null)
        {
            StartedDelegate = InAction.GetActionStartedDelegate();
            StartedDelegate += Activate;
            EndedDelegate = InAction.GetActionEndedDelegate();
            EndedDelegate += Deactivate;
        }
    }

    private void Activate(GameplayAction InAction)
    {
        StartedDelegate -= Activate;
    }


    public void Deactivate(GameplayAction InAction) 
    {
        EndedDelegate -= Deactivate;
    }

    public void Update()
    {
        
    }
}
