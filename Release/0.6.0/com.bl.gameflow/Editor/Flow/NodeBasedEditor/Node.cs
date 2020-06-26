using UnityEngine;
using System;
using UnityEditor;

namespace Bluel.UnityEditor
{
    public abstract class Node
    {
        public Rect rect;
        public string title;
        public bool isSelected { get; private set; }
        public Rect connRect {
            get {
                var r = new Rect(Vector2.zero, Vector2.one * 12);
                r.center = rect.center;
                return r;
            }
        }

        public GUIStyle style;
        public GUIStyle defaultNodeStyle;
        public GUIStyle selectedNodeStyle;

        public Node(Vector2 position, Vector2 size,
            GUIStyle nodeStyle, GUIStyle selectedStyle)
        {
            rect = new Rect(position, size);
            style = nodeStyle;
            defaultNodeStyle = nodeStyle;
            selectedNodeStyle = selectedStyle;
        }

        public void Drag(Vector2 delta)
        {
            rect.position += delta;
        }

        public void Draw()
        {
            GUI.Box(rect, title, style);
        }

        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (rect.Contains(e.mousePosition))
                        {
                            GUI.changed = true;
                            isSelected = true;
                            style = selectedNodeStyle;
                        }
                        else
                        {
                            GUI.changed = true;
                            isSelected = false;
                            style = defaultNodeStyle;
                        }
                    }
                    //if (e.button == 1 && isSelected && rect.Contains(e.mousePosition))
                    //{
                    //    ProcessContextMenu();
                    //    e.Use();
                    //}
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && isSelected)
                    {
                        Drag(e.delta);
                        e.Use();
                        return true;
                    }
                    break;
            }
            return false;
        }

        protected abstract void ProcessContextMenu();
    }
}