namespace GameFlow
{
    [NodeMenuItem("Composite/Sequencer",
        description: "This node is similar to AND operation, it will return Success when all children have returned Success.")]
    public sealed class Sequencer : CompositeNode
    {
        private int currentIndex;

        protected override void OnInit()
        {
            currentIndex = 0;
        }

        public override BehaviourStatus Tick()
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