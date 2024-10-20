using NodeCanvas.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuestStep : ActionTask
{
    public enum EQuestStepState
    {
        Unrevealed,
        Active,
        Completed,
    }

    [SerializeField] private string StepName;
    [SerializeField] private EQuestStepState QuestStepState;

    [SerializeField] private int CurrentCount;
    [SerializeField] private int MaxCount;


    public string GetQuestStepName()
    {
        if (MaxCount > 0)
        {
            return StepName + " " + CurrentCount + "/" + MaxCount;
        }
        return StepName;
    }

    public void IncrementCurrentCount()
    {
        CurrentCount++;
    }

    public void IncrementCurrentCount(int InIncrementCount)
    {
        CurrentCount += InIncrementCount;
    }
}
