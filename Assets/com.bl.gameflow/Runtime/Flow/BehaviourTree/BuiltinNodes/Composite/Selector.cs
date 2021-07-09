namespace GameFlow.Tree
{
    public sealed class Selector : ParentNode
    {
        /// <summary>
        /// [Composite/Selector]
        /// This node is similar to OR operation, it will return Success once a child have returned Success.
        /// </summary>
        public Selector(BehaviourNode[] children) : base(children) { }

        private int currentIndex;

        public override void Reset()
        {
            currentIndex = 0;
            base.Reset();
        }

        public override BehaviourStatus Tick()
        {
            var status = children[currentIndex].Tick();

            while (status == BehaviourStatus.Failure)
            {
                currentIndex++;
                if (currentIndex >= children.Length) break;
                status = children[currentIndex].Tick();
            }

            return status;
        }
    }
}