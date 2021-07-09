namespace GameFlow.Tree
{
    public sealed class Conditional : ParentNode
    {
        private System.Func<bool> predicate;

        /// <summary>
        /// [Composite/Conditional] Run its child when predicate is TRUE.
        /// </summary>
        public Conditional(System.Func<bool> predicate, BehaviourNode child) : base(child)
        {
            this.predicate = predicate;
        }

        public override BehaviourStatus Tick()
        {
            return predicate() ? child.Tick() : BehaviourStatus.Failure;
        }
    }
}