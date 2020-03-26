using UnityEditor;
using UnityEngine;

namespace GameFlow
{
    [CustomEditor(typeof(BehaviourTree), true)]
    internal class BehaviourTreeCustomEditor : BehaviourNodeCustomEditor
    {
        protected override bool CanAddChildItem(MonoBehaviour monoBehaviour)
        {
            return true;
        }

        protected override bool CanAddDecoratorItem(MonoBehaviour monoBehaviour)
        {
            return false;
        }
    }
}
