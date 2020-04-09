using GameFlow;
using UnityEditor;
using UnityEngine;

internal sealed class InteractionUnitCustomIcon : CustomHierarchyIcon<InteractionUnit>
{
    private static Texture2D activeIcon, inactiveIcon;

    [InitializeOnLoadMethod]
    static void Init()
    {
        activeIcon = LoadIconTex("Editor/GameFlow/signal_obj_active");
        inactiveIcon = LoadIconTex("Editor/GameFlow/signal_obj_inactive");
        new InteractionUnitCustomIcon();
    }

    protected override Texture2D GetIconTex(InteractionUnit t)
    {
        return t.isActiveAndEnabled ? activeIcon : inactiveIcon;
    }
}
