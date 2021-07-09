namespace GameFlow.Tree
{
    public sealed class Repeater : ParentNode
    {
        public int times { get; private set; }
        public bool endOnFailure { get; private set; }

        /// <summary>
        /// [Decorator/Repeater] This node will run its child specific times.
        /// </summary>
        public Repeater(int times, bool endOnFailure, BehaviourNode child) : base(child)
        {
            this.times = times;
            this.endOnFailure = endOnFailure;
        }

        private int currentTimes;

        public override BehaviourStatus Tick()
        {
            switch (child.Tick())
            {
                case BehaviourStatus.Running: break;
                case BehaviourStatus.Success:
                    {
                        child.Reset();
                        currentTimes++;
                        if (currentTimes >= times) return BehaviourStatus.Success;
                        break;
                    }
                case BehaviourStatus.Failure:
                    {
                        if (endOnFailure) return BehaviourStatus.Failure;
                        else
                        {
                            child.Reset();
                            currentTimes++;
                            if (currentTimes >= times) return BehaviourStatus.Success;
                            break;
                        }
                    }
            }
            return BehaviourStatus.Running;
        }

        public override void Reset()
        {
            currentTimes = 0;
            base.Reset();
        }
    }
}
