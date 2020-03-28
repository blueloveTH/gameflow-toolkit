using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFlow.AI
{
    public sealed class SimpleAction : BehaviourNode
    {
        public System.Action action { get; private set; }

        public SimpleAction SetAction(System.Action action)
        {
            this.action = action;
            return this;
        }

        internal override BehaviourStatus Tick()
        {
            action();
            return BehaviourStatus.Success;
        }
    }
}