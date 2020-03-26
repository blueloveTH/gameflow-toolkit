namespace GameFlow
{
    [NodeMenuItem("Decorator/Inverter",
        description: "This node is used to invert the result of its child.")]
    public sealed class Inverter : DecoratorNode
    {
        public override BehaviourStatus Tick()
        {
            int result = (int)child0.Tick();
            return (BehaviourStatus)(result * -1);
        }

        protected override void OnInit()
        {

        }
    }
}