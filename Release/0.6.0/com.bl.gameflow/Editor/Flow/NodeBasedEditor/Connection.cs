using System;
using UnityEditor;
using UnityEngine;

namespace Bluel.UnityEditor
{
    public class Connection
    {
        public Node source;
        public Node target;
        public Action<Connection> OnClickRemoveConnection;

        public Connection(Node source, Node target, Action<Connection> OnClickRemoveConnection)
        {
            this.source = source;
            this.target = target;
            this.OnClickRemoveConnection = OnClickRemoveConnection;
        }

        public void Draw()
        {
            Vector2 o, i;
            ConnRectUtil.CalcConnData(source.connRect, target.connRect, out o, out i);

            Handles.DrawAAPolyLine(5f, o, i);

            Vector2[] p = new Vector2[3];
            Vector2 v = (i - o).normalized * 8;
            Vector2 mid = (o + i) * 0.5f;
            p[0] = mid + v;
            p[1] = mid + (Vector2)(Quaternion.Euler(0, 0, 120f) * v);
            p[2] = mid + (Vector2)(Quaternion.Euler(0, 0, 240f) * v);

            Handles.DrawAAConvexPolygon(Array.ConvertAll<Vector2, Vector3>(p, (x) => x));

            //if (Handles.Button((o + i) * 0.5f,
            //    Quaternion.FromToRotation(Vector2.zero, i - o), 5, 5, Handles.RectangleHandleCap))
            //{

            //}
        }
    }
}