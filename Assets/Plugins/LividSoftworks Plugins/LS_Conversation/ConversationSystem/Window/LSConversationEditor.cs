// Engine Includes
using UnityEditor;
using UnityEngine.UIElements;

// Local Includes
using LS.Conversation.Utilities;

namespace LS.Conversation.Editor
{
    public class LSConversationEditor : EditorWindow
    {
        [MenuItem("Livid Softworks/CONVO/Conversation Editor")]
        public static void ShowExample()
        {
            GetWindow<LSConversationEditor>("Conversation Editor");
        }

        private void OnEnable()
        {
            AddGraphView();
            AddStyles();
        }

        private void AddGraphView()
        {
            LSConversationGrpahView ConvoGraph = new LSConversationGrpahView(this);
            ConvoGraph.StretchToParentSize();
            rootVisualElement.Add(ConvoGraph);
        }

        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("ConversationSystem/LSConversationVariables.uss");
        }
    }
}