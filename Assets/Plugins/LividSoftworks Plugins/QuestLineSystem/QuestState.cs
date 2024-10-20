using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NodeCanvas.Framework;
using Sirenix.OdinInspector;

[Serializable]
public abstract class QuestState : ActionTask
{
    [ShowInInspector, SerializeReference] public List<QuestTask> TransitionInQuestTasks = new();
    [ShowInInspector, SerializeReference] public List<QuestTask> TransitionOutQuestTasks = new();

    private int StateID;

    public virtual void EnterState()
    {
        TryExcuteQuestTasks(TransitionInQuestTasks);
    }

    public virtual void ExitState()
    {
        TryExcuteQuestTasks(TransitionOutQuestTasks);
    }

    private void TryExcuteQuestTasks(List<QuestTask> InQuestTasks)
    {
        foreach (QuestTask CurrentTask in InQuestTasks)
        {
            if (CurrentTask != null)
            {
                CurrentTask.ExcuteTask();
            }
        }
    }
}
