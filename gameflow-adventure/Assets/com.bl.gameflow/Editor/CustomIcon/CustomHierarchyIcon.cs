using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class CustomHierarchyIcon<T> where T : Component
{
    private Texture2D[] icons;
    private static List<CustomHierarchyIcon<T>> instances = new List<CustomHierarchyIcon<T>>();

    public CustomHierarchyIcon(params string[] iconPath)
    {
        icons = new Texture2D[iconPath.Length];
        for (int i = 0; i < icons.Length; i++)
            icons[i] = AssetDatabase.LoadAssetAtPath<Texture2D>(iconPath[i]);

        if (icons.Length == 0) return;
        EditorApplication.hierarchyWindowItemOnGUI += hierarchyItemOnGUI;
        EditorApplication.RepaintHierarchyWindow();
        instances.Add(this);
    }

    private void hierarchyItemOnGUI(int instanceID, Rect selectionRect)
    {
        GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

        if (go == null || icons.Length == 0) return;

        T t = go.GetComponent<T>();

        if (t != null)
        {
            Rect r = new Rect(selectionRect);
            r.x = r.width;

            int i = GetIconIndex(t);
            if (i < 0) return;
            GUI.Label(r, icons[i]);
        }
    }

    protected virtual int GetIconIndex(T t)
    {
        return 0;
    }
}
