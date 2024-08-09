using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventData_OnSetActiveQuest : EventData
{
    public QuestLine NewActiveQuest;
    public QuestLine PreviousActiveQuest;

    public override string GetEventType()
    {
        return "OnSetActiveQuest";
    }

    public override void SetData(params object[] args)
    {
        NewActiveQuest = (QuestLine)args[0];
        PreviousActiveQuest = (QuestLine)args[1];
    }
}
