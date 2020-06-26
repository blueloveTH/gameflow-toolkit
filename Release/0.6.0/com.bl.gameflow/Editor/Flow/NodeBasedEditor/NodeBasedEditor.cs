using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Bluel.UnityEditor
{
    public abstract class NodeBasedEditor : EditorWindow
    {
        protected List<Node> nodes = new List<Node>();
        protected List<Connection> connections = new List<Connection>();

        protected GUIStyle nodeStyle { get; private set; }
        protected GUIStyle selectedNodeStyle { get; private set; }

        private Vector2 offset;
        private Vector2 drag;

        protected virtual void OnGUI()
        {
            GUI.color = new Color(93 / 255f, 93 / 255f, 93 / 255f);
            GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), Texture2D.whiteTexture, ScaleMode.StretchToFill);
            GUI.color = Color.white;

            DrawGrid(20, new Color(0.33f, 0.33f, 0.33f));
            DrawGrid(100, new Color(0.25f, 0.25f, 0.25f));

            DrawConnections();
            DrawNodes();
            DrawConnectionLine(Event.current);

            ProcessNodeEvents(Event.current);
            ProcessEvents(Event.current);

            if (GUI.changed) Repaint();
        }

        private void DrawGrid(float spacing, Color color)
        {
            int widthDivs = Mathf.CeilToInt(position.width / spacing);
            int heightDivs = Mathf.CeilToInt(position.height / spacing);

            Handles.BeginGUI();
            Handles.color = color;

            offset += drag * 0.5f;
            Vector3 newOffset = new Vector3(offset.x % spacing, offset.y % spacing, 0);

            for (int i = 0; i < widthDivs; i++)
                Handles.DrawLine(new Vector3(spacing * i, -spacing, 0) + newOffset, new Vector3(spacing * i, position.height, 0f) + newOffset);

            for (int j = 0; j < heightDivs; j++)
                Handles.DrawLine(new Vector3(-spacing, spacing * j, 0) + newOffset, new Vector3(position.width, spacing * j, 0f) + newOffset);

            Handles.color = Color.white;
            Handles.EndGUI();
        }
        private void DrawConnectionLine(Event e)
        {
            //Handles.DrawBezier(
            //    selectedInPoint.rect.center,
            //    e.mousePosition,
            //    selectedInPoint.rect.center + Vector2.left * 50f,
            //    e.mousePosition - Vector2.left * 50f,
            //    Color.white,
            //    null,
            //    2f
            //);

            //GUI.changed = true;

            //Handles.DrawBezier(
            //    selectedOutPoint.rect.center,
            //    e.mousePosition,
            //    selectedOutPoint.rect.center - Vector2.left * 50f,
            //    e.mousePosition + Vector2.left * 50f,
            //    Color.white,
            //    null,
            //    2f
            //);

            GUI.changed = true;
        }
        private void DrawNodes()
        {
            for (int i = 0; i < nodes.Count; i++)
                nodes[i].Draw();
        }
        private void DrawConnections()
        {
            for (int i = 0; i < connections.Count; i++)
                connections[i].Draw();
        }

        protected virtual GUIStyle InitNodeStyle()
        {
            var nodeStyle = new GUIStyle();
            nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node0.png") as Texture2D;
            nodeStyle.border = new RectOffset(12, 12, 12, 12);
            nodeStyle.normal.textColor = Color.white;
            nodeStyle.alignment = TextAnchor.MiddleCenter;
            return nodeStyle;
        }
        protected virtual GUIStyle InitSelectedNodeStyle()
        {
            var selectedNodeStyle = new GUIStyle();
            selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node0 on.png") as Texture2D;
            selectedNodeStyle.normal.textColor = Color.white;
            selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);
            selectedNodeStyle.alignment = TextAnchor.MiddleCenter;
            return selectedNodeStyle;
        }

        protected virtual void OnEnable()
        {
            nodeStyle = InitNodeStyle();
            selectedNodeStyle = InitSelectedNodeStyle();
        }

        private void ProcessNodeEvents(Event e)
        {
            for (int i = nodes.Count - 1; i >= 0; i--)
            {
                bool guiChanged = nodes[i].ProcessEvents(e);
                if (guiChanged) GUI.changed = true;
            }
        }
        private void ProcessEvents(Event e)
        {
            drag = Vector2.zero;

            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 1) ProcessContextMenu(e);
                    break;
                case EventType.MouseDrag:
                    if (e.button == 2)
                        OnDrag(e.delta);
                    break;
            }
        }
        private void OnDrag(Vector2 delta)
        {
            drag = delta;
            for (int i = 0; i < nodes.Count; i++)
                nodes[i].Drag(delta);
            GUI.changed = true;
        }

        protected abstract void ProcessContextMenu(Event e);

        protected Node AddNode(Vector2 position)
        {
            Node newNode = CreateNode(position);
            nodes.Add(newNode);
            return newNode;
        }

        protected abstract Node CreateNode(Vector2 position);

        public virtual void Reset()
        {
            nodes.Clear();
            connections.Clear();
        }

        private void OnClickRemoveNode(Node node)
        {
            List<Connection> connectionsToRemove = new List<Connection>();

            for (int i = 0; i < connections.Count; i++)
            {
                if (connections[i].source == node || connections[i].target == node)
                    connectionsToRemove.Add(connections[i]);
            }

            for (int i = 0; i < connectionsToRemove.Count; i++)
                connections.Remove(connectionsToRemove[i]);

            connectionsToRemove.Clear();
            nodes.Remove(node);
        }
        private void OnClickRemoveConnection(Connection connection)
        {
            connections.Remove(connection);
        }
        protected void AddConnection(Node o, Node i)
        {
            connections.Add(new Connection(o, i, OnClickRemoveConnection));
        }

        protected abstract void OnDestroy();
    }
}