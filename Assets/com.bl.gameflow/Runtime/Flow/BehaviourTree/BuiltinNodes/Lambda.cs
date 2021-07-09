namespace GameFlow.Tree
{
    public sealed class Lambda : BehaviourNode
    {
        private System.Action lambda;

        /// <summary>
        /// [Misc/Lambda] Call a lambda expression and then return Success.
        /// </summary>
        public Lambda(System.Action lambda)
        {
            this.lambda = lambda;
        }

        public override void Reset()
        {

        }

        public override BehaviourStatus Tick()
        {
            lambda();
            return BehaviourStatus.Success;
        }
    }
}