namespace GameFlow.AI
{
    /// <summary>
    /// [Composite/Selector]
    /// This node is similar to OR operation, it will return Success once a child have returned Success.
    /// </summary>
    public sealed class Selector : ParentNode
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

            while (status == BehaviourStatus.Failure)
            {
                currentIndex++;
                if (currentIndex >= childCount) break;
                status = GetChild(currentIndex).Tick();
            }

            return status;
        }
    }
}