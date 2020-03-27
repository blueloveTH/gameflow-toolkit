namespace GameFlow.AI
{
    /// <summary>
    /// [Decorator/Inverter]
    /// This node is used to invert the result of its child.
    /// </summary>
    public sealed class Inverter : ParentNode
    {
        internal override BehaviourStatus Tick()
        {
            int result = (int)child0.Tick();
            return (BehaviourStatus)(result * -1);
        }
    }
}