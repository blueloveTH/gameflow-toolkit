using System.Collections.Generic;
using UnityEngine;

public static class TransformEx
{
    public static void SetRootPosition(this Transform t, Vector3 p)
    {
        Vector3 dv = p - t.position;
        t.position = p;

        for (int i = 0; i < t.childCount; i++)
        {
            t.GetChild(i).position -= dv;
        }
    }

    public static Vector3 GetCenterPos(this Transform t)
    {
        int cc = t.childCount;
        Vector3 pos = Vector3.zero;
        for (int i = 0; i < cc; i++)
        {
            pos += t.GetChild(i).position;
        }
        return pos / cc;
    }

    public static void DestroyAllChildren(this Transform root)
    {
        while (root.childCount > 0) Object.DestroyImmediate(root.GetChild(0).gameObject);
    }
    public static void DestroyAllChildrenFX(this Transform root)
    {
        while (root.childCount > 0) ObjectFX.DestroyImmediate(root.GetChild(0).gameObject);
    }
    public static void MoveChildren(this Transform src, Transform dest)
    {
        Transform[] contents = src.GetCpntsInDirectChildren<Transform>();
        for (int i = 0; i < contents.Length; i++)
            contents[i].SetParent(dest);
    }
    public static T GetCpntInDirectChildren<T>(this Transform transform) where T : Component
    {
        int childCount = transform.childCount;
        T temp;
        for (int i = 0; i < childCount; i++)
        {
            temp = transform.GetChild(i).GetComponent<T>();
            if (temp != null) return temp;
        }
        return null;
    }
    public static T[] GetCpntsInDirectChildren<T>(this Transform transform) where T : Component
    {
        List<T> results = new List<T>();
        GetCpntsInDirectChildren<T>(transform, results);
        return results.ToArray();
    }
    public static void GetCpntsInDirectChildren<T>(this Transform transform, List<T> results) where T : Component
    {
        results.Clear();
        int childCount = transform.childCount;
        T[] temp;
        for (int i = 0; i < childCount; i++)
        {
            temp = transform.GetChild(i).GetComponents<T>();
            if (temp != null) results.AddRange(temp);
        }
    }
    public static T GetCpntInDirectParent<T>(this Transform c) where T : Component
    {
        if (c.parent != null) return c.parent.GetComponent<T>();
        return null;
    }

    public static void SwapChildren(this Transform t, int i, int j)
    {
        if (i == j) return;
        var ti = t.GetChild(i);
        var tj = t.GetChild(j);
        tj.SetSiblingIndex(i);
        ti.SetSiblingIndex(j);
    }

    public static bool Contains(this RectTransform rt, Vector2 pos)
    {
        Rect rect = rt.rect;
        rect.position += (Vector2)rt.position;
        return rect.Contains(pos);
    }
}