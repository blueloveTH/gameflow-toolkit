namespace GameFlow.AI
{
    /// <summary>
    /// [Composite/Sequencer]
    /// This node is similar to AND operation, it will return Success when all children have returned Success.
    /// </summary>
    public sealed class Sequencer : ParentNode
    {
        private int currentIndex;

        internal override void Reset(BehaviourTree tree)
        {
            base.Reset(tree);
            currentIndex = 0;
        }

        internal override BehaviourStatus Tick()
        {
            var status = GetChild(currentIndex).Tick();

            while (status == BehaviourStatus.Success)
            {
                currentIndex++;
                if (currentIndex >= childCount) break;
                status = GetChild(currentIndex).Tick();
            }

            return status;
        }
    }
}