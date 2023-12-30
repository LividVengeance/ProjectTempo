// Engine Includes
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;

// Local Includes
using LS.Conversation.ConvoNode;
using LS.Conversation.Enumerations;
using LS.Conversation.Utilities;
using UnityEditor.Localization.Plugins.XLIFF.V12;

namespace LS.Conversation.Editor
{
    public class LSConversationGrpahView : GraphView
    {
        private LSConversationEditor EditorWindow;
        private LSConversationSearchWindow ConvoSearchWindow;

        public LSConversationGrpahView(LSConversationEditor InEditorWindow)
        {
            EditorWindow = InEditorWindow;

            AddGrpahManipulators();
            AddGridBackgound();
            AddSearchWindow();
            AddGraphStyles();
        }

        private void AddSearchWindow()
        {
            if (ConvoSearchWindow == null)
            {
                ConvoSearchWindow = new LSConversationSearchWindow();
                ConvoSearchWindow.Initialize(this);
            }

            nodeCreationRequest = Context => SearchWindow.Open(new SearchWindowContext(Context.screenMousePosition), ConvoSearchWindow);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> CompatiblePorts = new List<Port>();

            ports.ForEach(port =>
            {
                if (startPort == port || startPort.node == port.node || startPort.direction == port.direction)
                {
                    return;
                }
                CompatiblePorts.Add(port);  
            });

            return CompatiblePorts; 
        }

        private void AddGridBackgound()
        {
            GridBackground Background = new GridBackground();
            Background.StretchToParentSize();
            Insert(0, Background);
        }

        private void AddGraphStyles()
        {
            this.AddStyleSheets("ConversationSystem/LSConversationGraphStyles.uss", "ConversationSystem/LSConversationNodeStyles.uss");
        }

        private void AddGrpahManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Single Choice)", LSConversationDialogueType.SingleChoice));
            this.AddManipulator(CreateNodeContextualMenu("Add Node (Multiple Choice)", LSConversationDialogueType.MultipleChoice));

            this.AddManipulator(CreateGroupContextMenu());
        }

        private IManipulator CreateGroupContextMenu()
        {
            return new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent =>
                    AddElement(CreateGroup("New Dialogue Group", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))));
        }

        private IManipulator CreateNodeContextualMenu(string InActionTitle, LSConversationDialogueType InDialougeType)
        {
            return new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(InActionTitle, actionEvent => 
                    AddElement(CreateNode(InDialougeType, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))));
        }

        public Group CreateGroup(string InGroupTitle, Vector2 InPostion)
        {
            Group NewGroup = new Group()
            {
                title = InGroupTitle,
            };
            NewGroup.SetPosition(new Rect(InPostion, Vector2.zero));
            return NewGroup;
        }

        public LSConversationNodeBase CreateNode(LSConversationDialogueType InDialougeType, Vector2 InPosition)
        {
            Type NodeType = GetNodeType(InDialougeType);
            LSConversationNodeBase Node = (LSConversationNodeBase) Activator.CreateInstance(NodeType);
            Node.Iniitialize(InPosition);
            Node.Draw();
            AddElement(Node);
            return Node;
        }

        private Type GetNodeType(LSConversationDialogueType InDialougeType)
        {
            switch(InDialougeType)
            {
                case LSConversationDialogueType.SingleChoice:
                    {
                        return new LSConersationSingleChoiceNode().GetType();
                    }
                    case LSConversationDialogueType.MultipleChoice:
                    {
                        return new LSConversationMultipleChoiceNode().GetType();
                    }
                default:
                    {
                        Debug.LogWarning("CONVO Warning: Unable to find convesation type " + InDialougeType.ToString());
                        return new LSConversationNodeBase().GetType();
                    }
            }
        }

        public Vector2 GetLocalMousePosition(Vector2 InMousePosition, bool bOffsetWindowPosition = false)
        {
            Vector2 WorldMousePosition = InMousePosition;
            if (bOffsetWindowPosition)
            {
                WorldMousePosition -= EditorWindow.position.position;
            }

            return contentViewContainer.WorldToLocal(WorldMousePosition);
        }
    }
}