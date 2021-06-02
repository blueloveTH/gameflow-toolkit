using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorEx
{
    public static Vector2 OverrideX(this Vector2 v, float x)
    {
        v.x = x;
        return v;
    }

    public static Vector2 OverrideY(this Vector2 v, float y)
    {
        v.y = y;
        return v;
    }

    public static Vector3 OverrideX(this Vector3 v, float x)
    {
        v.x = x;
        return v;
    }

    public static Vector3 OverrideY(this Vector3 v, float y)
    {
        v.y = y;
        return v;
    }

    public static Vector3 OverrideZ(this Vector3 v, float z)
    {
        v.z = z;
        return v;
    }

    public static Vector2Int OverrideX(this Vector2Int v, int x)
    {
        v.x = x;
        return v;
    }

    public static Vector2Int OverrideY(this Vector2Int v, int y)
    {
        v.y = y;
        return v;
    }
}
