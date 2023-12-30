// Engine Includes
using UnityEditor.Experimental.GraphView;
using UnityEngine;

// Local Includes
using LS.Conversation.ConvoNode;
using LS.Conversation.Utilities;

namespace LS.Conversation.ConvoNode
{
    public class LSConersationSingleChoiceNode : LSConversationNodeBase
    {
        public override void Iniitialize(Vector2 InPosition)
        {
            base.Iniitialize(InPosition);

            DialogueType = Enumerations.LSConversationDialogueType.SingleChoice;
            DialogueChoices.Add("Next Dialogue");
        }

        public override void Draw()
        {
            base.Draw();

            // Output Container
            foreach (string Choice in DialogueChoices)
            {
                Port ChoicePort = this.CreatePort(Choice, Orientation.Horizontal, Direction.Output, Port.Capacity.Single);
                outputContainer.Add(ChoicePort);    
            }

            RefreshExpandedState();
        }
    }
}