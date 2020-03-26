using UnityEditor;

namespace GameFlow
{
    [InitializeOnLoad]
    internal sealed class BehaviourTreeIcon : CustomHierarchyIcon<BehaviourTree>
    {
        private BehaviourTreeIcon() : base("Packages/com.bl.gameflow-core/Gizmos/behaviour_tree_icon.png") { }

        static BehaviourTreeIcon()
        {
            new BehaviourTreeIcon();
        }
    }
}