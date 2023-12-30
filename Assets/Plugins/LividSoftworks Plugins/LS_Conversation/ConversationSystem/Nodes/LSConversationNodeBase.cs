// Engine Includes
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

// Local Includes
using LS.Conversation.Utilities;

namespace LS.Conversation.ConvoNode
{
    using Enumerations;
    using Pathfinding.Util;
    using System;
    using System.Numerics;
    using UnityEditor;
    using UnityEngine.Networking.Types;
    using UnityEngine.UIElements;
 

    public class LSConversationNodeBase : Node
    {
        public string DialogueName { get; set; }
        public List<string> DialogueChoices { get; set; }
        public string Text { get; set; }
        public LSConversationDialogueType DialogueType { get; set; }
        public System.Guid NodeGUID;

        public virtual void Iniitialize(UnityEngine.Vector2 InPosition)
        {
            DialogueName = "DialougName";
            DialogueChoices = new List<string>();
            Text = "Dialogue Text.";
            NodeGUID = System.Guid.NewGuid();

            SetPosition(new Rect(InPosition, UnityEngine.Vector2.zero));

            mainContainer.AddToClassList("convo-node__main-container");
            extensionContainer.AddToClassList("convo-node__extenstion-container");
        }

        public virtual void Draw()
        {
            // Title Container
            TextField DialougNameTextField = LSConversationNodeUtilities.CreateTextField(DialogueName);

            DialougNameTextField.AddClasses("convo-node__textfield", "convo-node__filename-textfield", "convo-node__textfield__hidden");
            titleContainer.Insert(0, DialougNameTextField);

            // Input Container
            Port InputPort = this.CreatePort("Dialouge Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            inputContainer.Add(InputPort);

            // Extensions Container
            VisualElement CustomDataContainer = new VisualElement();
            CustomDataContainer.AddToClassList("convo-node__custom-data-container");

            Foldout TextFoldout = LSConversationNodeUtilities.CreateFoldout("Dialouge Text");

            TextField TextTextField = LSConversationNodeUtilities.CreateTextArea(Text);
            TextTextField.AddClasses("convo-node__textfield", "convo-node__quote-textfield");

            TextFoldout.Add(TextTextField);
            
            CustomDataContainer.Add(TextFoldout);
            extensionContainer.Add(CustomDataContainer);

            RefreshExpandedState();
        }

        public System.Guid GetNodeID()
        {
            return NodeGUID;
        }
    }
}