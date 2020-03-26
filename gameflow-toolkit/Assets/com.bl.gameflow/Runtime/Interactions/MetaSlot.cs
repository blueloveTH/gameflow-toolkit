using UnityEngine;

namespace GameFlow
{
    internal struct MetaSlot
    {
        public MonoBehaviour target { get; private set; }
        public string methodName { get; private set; }

        public MetaSlot(MonoBehaviour target, string methodName)
        {
            this.target = target;
            this.methodName = methodName;
        }
    }
}