using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Framework;

[Serializable]
public abstract class QuestState : ActionTask
{
    private int StateID;

    public virtual void EnterState()
    {

    }

    public virtual void ExitState()
    {

    }
}
