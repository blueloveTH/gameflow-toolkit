using GameFlow;
using UnityEditor;

[InitializeOnLoad]
internal sealed class ModelNodeCustomHierarchy : CustomHierarchyIcon<InteractionHeader>
{
    ModelNodeCustomHierarchy(string iconPath) : base(iconPath) { }

    static ModelNodeCustomHierarchy()
    {
        new ModelNodeCustomHierarchy("Packages/GameFlow/Gizmos/modelnode.psd");
    }

    protected override int GetIconIndex(InteractionHeader t)
    {
        if (t.transform.parent != null) return -1;
        return base.GetIconIndex(t);
    }
}
