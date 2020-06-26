using Bluel.UnityEditor;
using System;
using UnityEditor;
using UnityEngine;

namespace GameFlow
{
    public class FSMEditorNode : Node
    {
        public event Action<Node> OnRemoveNode;

        public FSMEditorNode(Vector2 position, Vector2 size,
            GUIStyle nodeStyle, GUIStyle selectedStyle) :
            base(position, size, nodeStyle, selectedStyle)
        {

        }

        protected override void ProcessContextMenu()
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Delete State"), false, () => OnRemoveNode?.Invoke(this));
            genericMenu.ShowAsContext();
        }
    }
}
