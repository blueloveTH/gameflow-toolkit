using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class CustomHierarchyIcon<T> where T : Component
{
    //private static List<CustomHierarchyIcon<T>> instances = new List<CustomHierarchyIcon<T>>();

    public CustomHierarchyIcon()
    {
        EditorApplication.hierarchyWindowItemOnGUI += hierarchyItemOnGUI;
        EditorApplication.RepaintHierarchyWindow();

        //instances.Add(this);
    }

    protected static Texture2D LoadIconTex(string resPath)
    {
        return Resources.Load<Texture2D>(resPath);
    }

    protected abstract Texture2D GetIconTex(T t);

    private void hierarchyItemOnGUI(int instanceID, Rect selectionRect)
    {
        GameObject go = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (go == null) return;

        T t = go.GetComponent<T>();
        if (t == null) return;

        var tex = GetIconTex(t);

        if (tex != null)
        {
            Rect r = new Rect(selectionRect);
            r.x = r.width;
            GUI.Label(r, tex);
        }
    }
}
