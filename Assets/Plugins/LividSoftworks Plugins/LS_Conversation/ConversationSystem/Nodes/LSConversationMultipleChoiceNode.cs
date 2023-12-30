// Engine Includes
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

// Local Includes
using LS.Conversation.Enumerations;
using LS.Conversation.Utilities;

namespace LS.Conversation.ConvoNode
{
    public class LSConversationMultipleChoiceNode : LSConversationNodeBase
    {
        public override void Iniitialize(Vector2 InPosition)
        {
            base.Iniitialize(InPosition);
            DialogueType = LSConversationDialogueType.MultipleChoice;
            DialogueChoices.Add("New Choice");
        }

        public override void Draw()
        {
            base.Draw();

            // Main Container
            Button AddChoiceButton = LSConversationNodeUtilities.CreateButton("Add Choice", () =>
            {
                CreateChoicePort("New Choice");
                DialogueChoices.Add("New Choice");
            });

            AddChoiceButton.AddClasses("convo-node__button");
            mainContainer.Insert(1, AddChoiceButton);

            // Output Container
            foreach (string Choice in DialogueChoices)
            {
                CreateChoicePort(Choice);
            }

            RefreshExpandedState();
        }

        private void CreateChoicePort(string InChoiceName)
        {
            Port ChoicePort = this.CreatePort();

            Button DeleteChoiceButton = LSConversationNodeUtilities.CreateButton("Del");
            DeleteChoiceButton.AddClasses("convo-node__button");

            TextField ChoiceTextFeild = LSConversationNodeUtilities.CreateTextField(InChoiceName);
            ChoiceTextFeild.AddClasses("convo-node__textfield", "convo-node__choice-textfield", "convo-node__textfield__hidden");

            ChoicePort.Add(ChoiceTextFeild);
            ChoicePort.Add(DeleteChoiceButton);

            outputContainer.Add(ChoicePort);
        }
    }
}