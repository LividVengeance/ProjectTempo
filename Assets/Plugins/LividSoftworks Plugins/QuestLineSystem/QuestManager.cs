using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private QuestLine CurrentActiveQuest;
    private QuestLine PreviousActiveQuest;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool SetActiveQuest(QuestLine InNewActiveQuest)
    {
        bool bUpdatedActiveQuest = false;

        if (CurrentActiveQuest != null)
        {
            PreviousActiveQuest = CurrentActiveQuest;
            CurrentActiveQuest = InNewActiveQuest;

            EventManager.Instance.FireEvent<EventData_OnSetActiveQuest>(CurrentActiveQuest, PreviousActiveQuest);
            bUpdatedActiveQuest = true;
        }

        return bUpdatedActiveQuest;
    }
}
