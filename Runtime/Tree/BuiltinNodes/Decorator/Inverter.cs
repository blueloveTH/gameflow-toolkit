namespace GameFlow.Tree
{
    public sealed class Inverter : ParentNode
    {
        /// <summary>
        /// [Decorator/Inverter]
        /// This node is used to invert the result of its child.
        /// </summary>
        public Inverter(BehaviourNode child) : base(child) { }

        public override BehaviourStatus Tick()
        {
            int result = (int)child.Tick();
            return (BehaviourStatus)(result * -1);
        }
    }
}