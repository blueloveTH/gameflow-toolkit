using System.Collections.Generic;

namespace GameFlow.AI
{
    /// <summary>
    /// [Composite/Parallel]
    /// This node is similar to Sequencer, but simultaneously.
    /// </summary>
    public sealed class Parallel : ParentNode
    {
        private HashSet<BehaviourNode> rest;

        internal override BehaviourStatus Tick()
        {
            List<BehaviourNode> succeed = new List<BehaviourNode>();
            foreach (var item in rest)
            {
                switch (item.Tick())
                {
                    case BehaviourStatus.Failure: return BehaviourStatus.Failure;
                    case BehaviourStatus.Success: succeed.Add(item); break;
                    case BehaviourStatus.Running: break;
                }
            }
            rest.ExceptWith(succeed);
            if (rest.Count == 0) return BehaviourStatus.Success;
            return BehaviourStatus.Running;
        }

        internal override void Reset(BehaviourTree tree)
        {
            base.Reset(tree);
            rest = new HashSet<BehaviourNode>(GetChildNodes());
        }
    }
}
