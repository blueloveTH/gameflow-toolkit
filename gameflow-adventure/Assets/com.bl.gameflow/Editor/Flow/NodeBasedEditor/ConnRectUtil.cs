using UnityEngine;

namespace Bluel.UnityEditor
{
    internal static class ConnRectUtil
    {
        public static void CalcConnData(Rect connRect0, Rect connRect1, out Vector2 o, out Vector2 i)
        {
            o = i = Vector2.zero;
            Vector2[,] p = new Vector2[2, 4];
            p[0, 0] = new Vector2(connRect0.xMin, connRect0.yMax);
            p[0, 1] = connRect0.max;
            p[0, 2] = connRect0.min;
            p[0, 3] = new Vector2(connRect0.xMax, connRect0.yMin);

            p[1, 0] = new Vector2(connRect1.xMin, connRect1.yMax);
            p[1, 1] = connRect1.max;
            p[1, 2] = connRect1.min;
            p[1, 3] = new Vector2(connRect1.xMax, connRect1.yMin);

            float d31 = (p[0, 3] - p[1, 1]).SqrMagnitude();
            float d13 = (p[0, 1] - p[1, 3]).SqrMagnitude();
            float d32 = (p[0, 3] - p[1, 2]).SqrMagnitude();
            float d23 = (p[0, 2] - p[1, 3]).SqrMagnitude();

            float minD = Mathf.Min(d31, d13, d32, d23);

            if (minD == d31)
            {
                o = p[0, 2];
                i = p[1, 0];
            }

            if (minD == d13)
            {
                o = p[0, 1];
                i = p[1, 3];
            }

            if (minD == d32)
            {
                o = p[0, 3];
                i = p[1, 2];
            }

            if (minD == d23)
            {
                o = p[0, 0];
                i = p[1, 1];
            }
        }
    }
}
