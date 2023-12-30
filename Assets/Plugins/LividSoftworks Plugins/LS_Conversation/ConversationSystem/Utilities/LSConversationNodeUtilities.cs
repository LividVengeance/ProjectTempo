// Engine Includes
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

// Local Includes
using LS.Conversation.ConvoNode;

namespace LS.Conversation.Utilities
{
    public static class LSConversationNodeUtilities
    {
        public static TextField CreateTextField(string InValue = null, EventCallback<ChangeEvent<string>> OnValueChanged = null)
        {
            TextField NewTextField = new TextField()
            {
                value = InValue,
            };

            if (OnValueChanged != null)
            {
                NewTextField.RegisterValueChangedCallback(OnValueChanged);
            }
            return NewTextField;
        }

        public static TextField CreateTextArea(string InValue = null, EventCallback<ChangeEvent<string>> OnValueChanged = null)
        {
            TextField TextFieldArea = CreateTextField(InValue, OnValueChanged);
            TextFieldArea.multiline = true;
            return TextFieldArea;
        }

        public static Foldout CreateFoldout(string InTitle, bool bInCollapsed = false)
        {
            Foldout NewFoldout = new Foldout()
            {
                text = InTitle,
                value = !bInCollapsed,
            };

            return NewFoldout;
        }

        public static Button CreateButton(string InTitle, Action OnClick = null)
        {
            Button NewButton = new Button(OnClick)
            {
                text = InTitle,
            };
            
            return NewButton;
        }

        public static Port CreatePort(this LSConversationNodeBase InNode, string InPortName = "", Orientation InOrientation = Orientation.Horizontal, 
            Direction InDirection = Direction.Output, Port.Capacity InCapacity = Port.Capacity.Single)
        {
            Port NewPort = InNode.InstantiatePort(InOrientation, InDirection, InCapacity, typeof(bool));
            NewPort.portName = InPortName;
            return NewPort;
        }
    }
}