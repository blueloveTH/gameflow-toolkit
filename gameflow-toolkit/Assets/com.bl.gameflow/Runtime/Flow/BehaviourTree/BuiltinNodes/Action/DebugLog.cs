using UnityEngine;

namespace GameFlow
{
    [NodeMenuItem("Action/Debug.Log")]
    public class DebugLog : ActionNode
    {
        public string message = "";

        public override BehaviourStatus Tick()
        {
            print(message);
            return BehaviourStatus.Success;
        }

        protected override void OnInit()
        {

        }
    }
}