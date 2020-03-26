using UnityEditor;

namespace GameFlow
{
    [InitializeOnLoad]
    internal sealed class MsgObjHierarchyIcon : CustomHierarchyIcon<InteractionUnit>
    {
        MsgObjHierarchyIcon(params string[] iconPath) : base(iconPath) { }
        static MsgObjHierarchyIcon()
        {
            new MsgObjHierarchyIcon(
                "Packages/GameFlow/Gizmos/MsgObj/msg_rs.psd",
                "Packages/GameFlow/Gizmos/MsgObj/msg_r.psd",
                "Packages/GameFlow/Gizmos/MsgObj/msg_s.psd",
                "Packages/GameFlow/Gizmos/MsgObj/msg_x.psd"
                );
        }

        protected override int GetIconIndex(InteractionUnit t)
        {
            if (t.gameObject.activeSelf) return 3;
            return 0;
        }
    }
}