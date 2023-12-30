// Engine Includes
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

// Local Includes
using LS.Conversation.Enumerations;
using LS.Conversation.ConvoNode;

namespace LS.Conversation.Editor
{
    public class LSConversationSearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private LSConversationGrpahView GraphView;
        private Texture2D IndentaionTexture;

        public void Initialize(LSConversationGrpahView InGraphView)
        {
            GraphView = InGraphView;
            IndentaionTexture = new Texture2D(1, 1);
            IndentaionTexture.SetPixel(0, 0, Color.clear);
            IndentaionTexture.Apply();
        }

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> SearchTreeEntries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Create Node")),
                new SearchTreeGroupEntry(new GUIContent("Dialouge Node"), 1),
                new SearchTreeEntry(new GUIContent("Single Choice", IndentaionTexture))
                {
                    level = 2,
                    userData = LSConversationDialogueType.SingleChoice,
                },
                new SearchTreeEntry(new GUIContent("Multiple Choice", IndentaionTexture))
                {
                    level = 2,
                    userData = LSConversationDialogueType.SingleChoice,
                },
                new SearchTreeGroupEntry(new GUIContent("Dialogue Group"), 1),
                new SearchTreeEntry(new GUIContent("Single Group", IndentaionTexture))
                {
                    level = 2,
                    userData = new Group()
                }
            };  
            return SearchTreeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            Vector2 LocalMousePosition = GraphView.GetLocalMousePosition(context.screenMousePosition, true);

            switch (SearchTreeEntry.userData)
            {
                case LSConversationDialogueType.SingleChoice:
                    {
                        LSConersationSingleChoiceNode SingleChoiceNode = (LSConersationSingleChoiceNode) GraphView.CreateNode(
                            LSConversationDialogueType.SingleChoice, LocalMousePosition);
                        GraphView.AddElement(SingleChoiceNode); 
                        return true;
                    }
                case LSConversationDialogueType.MultipleChoice:
                    {
                        LSConversationMultipleChoiceNode MultipleChoiceNode = (LSConversationMultipleChoiceNode) GraphView.CreateNode(
                            LSConversationDialogueType.MultipleChoice, LocalMousePosition);
                        GraphView.AddElement(MultipleChoiceNode);
                        return true;
                    }
                case Group _:
                    {
                        Group NewGroup = GraphView.CreateGroup("New Group", LocalMousePosition);
                        GraphView.AddElement(NewGroup);
                        return true;
                    }
                    default:
                    {
                        Debug.LogWarning("LS.Convo_Search-Window| Unable to find entry: " + SearchTreeEntry.userData);
                        return false;
                    }
            }
        }
    }
}