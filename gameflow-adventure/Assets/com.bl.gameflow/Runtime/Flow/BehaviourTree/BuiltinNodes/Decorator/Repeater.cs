namespace GameFlow.AI
{
    /// <summary>
    /// [Decorator/Repeater]
    /// This node will run its child specific times.
    /// </summary>
    public sealed class Repeater : ParentNode
    {
        public int times { get; set; }
        public bool endOnFailure { get; set; }

        private int currentTimes;

        internal override BehaviourStatus Tick()
        {
            switch (child0.Tick())
            {
                case BehaviourStatus.Running: break;
                case BehaviourStatus.Success:
                    {
                        child0.Reset(tree);
                        currentTimes++;
                        if (currentTimes >= times) return BehaviourStatus.Success;
                        break;
                    }
                case BehaviourStatus.Failure:
                    {
                        if (endOnFailure) return BehaviourStatus.Failure;
                        else
                        {
                            child0.Reset(tree);
                            currentTimes++;
                            if (currentTimes >= times) return BehaviourStatus.Success;
                            break;
                        }
                    }
            }
            return BehaviourStatus.Running;
        }

        internal override void Reset(BehaviourTree tree)
        {
            base.Reset(tree);
            currentTimes = 0;
        }
    }
}
