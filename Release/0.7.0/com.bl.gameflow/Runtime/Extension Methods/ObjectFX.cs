using UnityEngine;

public static class ObjectFX
{
    /// <summary>
    /// Destroy a GameObject and broadcast message "OnDestroyFX", 
    /// because we cannot spawn objects in unity built-in "OnDestroy"
    /// </summary>
    public static void Destroy(GameObject go)
    {
        go.BroadcastMessage("OnDestroyFX", SendMessageOptions.DontRequireReceiver);
        Object.Destroy(go);
    }

    /// <summary>
    /// Destroy a GameObject immediately and broadcast message "OnDestroyFX", 
    /// because we cannot spawn objects in unity built-in "OnDestroy"
    /// </summary>
    public static void DestroyImmediate(GameObject go)
    {
        go.BroadcastMessage("OnDestroyFX", SendMessageOptions.DontRequireReceiver);
        Object.DestroyImmediate(go);
    }
}