// Engine Includes
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

// Local Includes

namespace LS.Conversation.Utilities
{
    public static class LSConversationStylesUtilities
    {
        public static VisualElement AddStyleSheets(this VisualElement InElement, params string[] InStyleSheetNames)
        {
            foreach (string StyleSheetName in InStyleSheetNames)
            {
                StyleSheet NodeStyleSheet = (StyleSheet) EditorGUIUtility.Load(StyleSheetName);
                InElement.styleSheets.Add(NodeStyleSheet);
            }
            return InElement;
        }

        public static VisualElement AddClasses(this VisualElement InElement, params string[] InClassNames)
        {
            foreach (string ClassName in InClassNames)
            {
                InElement.AddToClassList(ClassName);
            }
            return InElement;
        }
    }
}