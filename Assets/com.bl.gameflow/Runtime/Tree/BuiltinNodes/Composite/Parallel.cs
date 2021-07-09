using System.Collections.Generic;

namespace GameFlow.Tree
{
    public sealed class Parallel : ParentNode
    {
        /// <summary>
        /// [Composite/Parallel]
        /// This node is similar to Sequencer, but all children will be ticked simultaneously.
        /// </summary>
        public Parallel(BehaviourNode[] children) : base(children) { }

        private HashSet<BehaviourNode> restChildren;

        public override BehaviourStatus Tick()
        {
            HashSet<BehaviourNode> succeed = new HashSet<BehaviourNode>();
            foreach (var item in restChildren)
            {
                switch (item.Tick())
                {
                    case BehaviourStatus.Failure: return BehaviourStatus.Failure;
                    case BehaviourStatus.Success: succeed.Add(item); break;
                    case BehaviourStatus.Running: break;
                }
            }
            restChildren.ExceptWith(succeed);
            if (restChildren.Count == 0) return BehaviourStatus.Success;
            return BehaviourStatus.Running;
        }

        public override void Reset()
        {
            restChildren = new HashSet<BehaviourNode>(children);
            base.Reset();
        }
    }
}
