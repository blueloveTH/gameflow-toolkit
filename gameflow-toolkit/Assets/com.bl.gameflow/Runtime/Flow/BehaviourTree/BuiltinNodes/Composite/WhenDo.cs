using UnityEngine;

namespace GameFlow
{
    [NodeMenuItem("Composite/WhenDo", "This node will exec child node when all conditions are established.")]
    public class WhenDo : ParentNode
    {
        public abstract class Condition : MonoBehaviour
        {
            public abstract bool Check(IGameModel model);
        }

        private Condition[] conditions;

        protected override void Awake()
        {
            base.Awake();
            conditions = GetComponents<Condition>();
        }

        protected override void OnInit()
        {

        }

        public override BehaviourStatus Tick()
        {
            if (Check()) return child0.Tick();
            else return BehaviourStatus.Failure;
        }

        public bool Check()
        {
            for (int i = 0; i < conditions.Length; i++)
            {
                if (!conditions[i].Check(tree.model)) return false;
            }
            return true;
        }
    }
}