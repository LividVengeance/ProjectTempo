using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

namespace LSConversation
{
    public static class EventNames
    {
        public static string ConversationRoot = "Conversation Root";
    }

    [UnitTitle("Conversation Root")]
    [UnitCategory("Conversation\\Events")]
    public class SGConversationRoot : EventUnit<int>
    {
        [DoNotSerialize]
        public ValueOutput result { get; private set; }// The Event output data to return when the Event is triggered.
        protected override bool register => true;

        // Add an EventHook with the name of the Event to the list of Visual Scripting Events.
        public override EventHook GetHook(GraphReference reference)
        {
            return new EventHook(EventNames.ConversationRoot);
        }
        protected override void Definition()
        {
            base.Definition();
            // Setting the value on our port.
            result = ValueOutput<int>(nameof(result));
        }
        // Setting the value on our port.
        protected override void AssignArguments(Flow flow, int data)
        {
            flow.SetValue(result, data);
        }
    }
}