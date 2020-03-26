namespace GameFlow
{
    [NodeMenuItem("Composite/Selector",
        description: "This node is similar to OR operation, it will return Success once a child have returned Success.")]
    public sealed class Selector : CompositeNode
    {
        private int currentIndex;

        protected override void OnInit()
        {
            currentIndex = 0;
        }

        public override BehaviourStatus Tick()
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