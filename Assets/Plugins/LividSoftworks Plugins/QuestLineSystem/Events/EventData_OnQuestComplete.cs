using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventData_OnQuestComplete : EventData
{
    public QuestLine CompletedActiveQuest;

    public override string GetEventType()
    {
        return "OnQuestComplete";
    }

    public override void SetData(params object[] args)
    {
        CompletedActiveQuest = (QuestLine)args[0];
    }
}
