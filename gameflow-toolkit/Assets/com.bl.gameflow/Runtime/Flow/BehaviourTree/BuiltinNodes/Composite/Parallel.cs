using System.Collections.Generic;

namespace GameFlow
{
    [NodeMenuItem("Composite/Parallel",
        description: "This node is similar to Sequencer, but simultaneously.")]
    public sealed class Parallel : CompositeNode
    {
        private HashSet<BehaviourNode> rest;

        public override BehaviourStatus Tick()
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

        protected override void OnInit()
        {
            rest = new HashSet<BehaviourNode>(GetChildNodes());
        }
    }
}
