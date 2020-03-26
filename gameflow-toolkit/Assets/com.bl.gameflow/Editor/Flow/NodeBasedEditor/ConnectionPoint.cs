using System;
using UnityEngine;

namespace Bluel.UnityEditor
{
    public enum ConnPointType { In, Out }

    public class ConnectionPoint
    {
        public Rect rect;
        public ConnPointType type;
        public Node node;
        public GUIStyle style;

        public ConnectionPoint(Node node, ConnPointType type, GUIStyle style)
        {
            this.node = node;
            this.type = type;
            this.style = style;
            rect = new Rect(0, 0, 8f, 8f);
        }

        public void Draw()
        {
            rect.y = node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f;

            switch (type)
            {
                case ConnPointType.Out:
                    rect.x = node.rect.x + node.rect.width * 0.5f - rect.width;
                    break;
                case ConnPointType.In:
                    rect.x = node.rect.x + node.rect.width * 0.5f + rect.width;
                    break;
            }
        }
    }
}