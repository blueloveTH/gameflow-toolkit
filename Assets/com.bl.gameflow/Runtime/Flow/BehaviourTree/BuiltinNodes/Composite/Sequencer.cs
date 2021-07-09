namespace GameFlow.Tree
{
    public sealed class Sequencer : ParentNode
    {
        /// <summary>
        /// [Composite/Sequencer]
        /// This node is similar to AND operation, it will return Success when all children have returned Success.
        /// </summary>
        public Sequencer(BehaviourNode[] children) : base(children) { }

        private int currentIndex;

        public override void Reset()
        {
            currentIndex = 0;
            base.Reset();
        }

        public override BehaviourStatus Tick()
        {
            var status = children[currentIndex].Tick();

            while (status == BehaviourStatus.Success)
            {
                currentIndex++;
                if (currentIndex >= children.Length) break;
                status = children[currentIndex].Tick();
            }

            return status;
        }
    }
}