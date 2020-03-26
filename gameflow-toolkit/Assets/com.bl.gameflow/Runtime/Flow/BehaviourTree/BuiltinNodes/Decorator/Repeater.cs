using UnityEngine;

namespace GameFlow
{
    [NodeMenuItem("Decorator/Repeater",
        description: "This node will run its child specific times.")]
    public sealed class Repeater : DecoratorNode
    {
        [SerializeField] int times;
        [SerializeField] bool endOnFailure;

        private int currentTimes;

        public override BehaviourStatus Tick()
        {
            switch (child0.Tick())
            {
                case BehaviourStatus.Running: break;
                case BehaviourStatus.Success:
                    {
                        child0.Init();
                        currentTimes++;
                        if (currentTimes >= times) return BehaviourStatus.Success;
                        break;
                    }
                case BehaviourStatus.Failure:
                    {
                        if (endOnFailure) return BehaviourStatus.Failure;
                        else
                        {
                            child0.Init();
                            currentTimes++;
                            if (currentTimes >= times) return BehaviourStatus.Success;
                            break;
                        }
                    }
            }
            return BehaviourStatus.Running;
        }

        protected override void OnInit()
        {
            currentTimes = 0;
        }
    }
}
