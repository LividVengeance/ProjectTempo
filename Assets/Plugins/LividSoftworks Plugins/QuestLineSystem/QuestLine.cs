using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "QL_QuestLineAsset", menuName = "Quests/QuestLine")]
public class QuestLine : ScriptableObject
{
    [SerializeField] private string QuestName;
    [SerializeField] private string ShortDescription;
    [SerializeField] private string LongDescription;

    [Space] [SerializeField] public List<QuestStep> QuestSteps = new();
    [Space] [ShowInInspector, SerializeReference] public List<QuestState> QuestLineStates = new();

    // TODO: Save out the state ID and set the current state on load
    private QuestState CurrentActiveQuestState;
    private QuestState PreviousActiveQuestState;

    public bool SetActiveQuestState(QuestState InQuestState)
    {
        bool bUpdatedActiveState = false;

        if (InQuestState != null)
        {
            PreviousActiveQuestState = CurrentActiveQuestState;
            CurrentActiveQuestState = InQuestState;
            InQuestState.EnterState();
            bUpdatedActiveState = true;
        }

        return bUpdatedActiveState;
    }

    public QuestState GetActiveQuestState()
    {
        return CurrentActiveQuestState;
    }
}
