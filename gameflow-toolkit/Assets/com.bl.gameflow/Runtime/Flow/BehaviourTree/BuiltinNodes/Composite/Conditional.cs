using System.Collections.Generic;
using UnityEngine;

namespace GameFlow.AI
{
    public sealed class Conditional : ParentNode
    {
        private System.Func<bool> checkFunc;

        public Conditional SetFunc(System.Func<bool> checkFunc)
        {
            this.checkFunc = checkFunc;
            return this;
        }

        internal override BehaviourStatus Tick()
        {
            if (checkFunc())
            {
                if (childCount > 0) return child0.Tick();
                else return BehaviourStatus.Success;
            }
            else return BehaviourStatus.Failure;
        }
    }
}