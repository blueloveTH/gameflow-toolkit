using GameFlow;
using UnityEditor;
using UnityEngine;

internal sealed class InteractionNodeCustomIcon : CustomHierarchyIcon<InteractionNode>
{
    private static Texture2D activeIcon, inactiveIcon;

    [InitializeOnLoadMethod]
    static void Init()
    {
        activeIcon = LoadIconTex("Editor/GameFlow/signal_obj_active");
        inactiveIcon = LoadIconTex("Editor/GameFlow/signal_obj_inactive");
        new InteractionNodeCustomIcon();
    }

    protected override Texture2D GetIconTex(InteractionNode t)
    {
        return t.isActiveAndEnabled ? activeIcon : inactiveIcon;
    }
}
