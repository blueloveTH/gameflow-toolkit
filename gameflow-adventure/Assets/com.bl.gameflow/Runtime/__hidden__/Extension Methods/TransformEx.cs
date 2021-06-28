using System.Collections.Generic;
using UnityEngine;

public static class TransformEx
{
    /// <summary>
    /// Set position for a root Transform object and keep its children's position unchanged.
    /// </summary>
    public static void SetRootPosition(this Transform t, Vector3 p)
    {
        Vector3 dv = p - t.position;
        t.position = p;

        for (int i = 0; i < t.childCount; i++)
            t.GetChild(i).position -= dv;
    }

    public static void DestroyAllChildren(this Transform root)
    {
        while (root.childCount > 0) Object.DestroyImmediate(root.GetChild(0).gameObject);
    }

    public static void DestroyAllChildrenFX(this Transform root)
    {
        while (root.childCount > 0) ObjectFX.DestroyImmediate(root.GetChild(0).gameObject);
    }

    public static void MoveChildren(this Transform src, Transform dest, bool worldPostionStays = false)
    {
        Transform[] contents = src.GetCpntsInDirectChildren<Transform>();
        for (int i = 0; i < contents.Length; i++)
            contents[i].SetParent(dest, worldPostionStays);
    }

    public static T GetCpntInDirectChildren<T>(this Transform transform) where T : Component
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            T temp;
            if (transform.GetChild(i).TryGetComponent(out temp))
                return temp;
        }
        return null;
    }

    public static T[] GetCpntsInDirectChildren<T>(this Transform transform) where T : Component
    {
        List<T> results = new List<T>();
        for (int i = 0; i < transform.childCount; i++)
        {
            T[] temp = transform.GetChild(i).GetComponents<T>();
            if (temp != null) results.AddRange(temp);
        }
        return results.ToArray();
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
}