namespace GameFlow.Tree
{
    public abstract class BehaviourNode
    {
        public abstract BehaviourStatus Tick();
        public abstract void Reset();
    }

    public abstract class ParentNode : BehaviourNode
    {
        protected BehaviourNode[] children { get; private set; }
        protected BehaviourNode child => children[0];

        public ParentNode(BehaviourNode[] children)
        {
            this.children = children;
        }

        public ParentNode(BehaviourNode child)
        {
            this.children = new BehaviourNode[] { child };
        }

        public override void Reset()
        {
            Apply((x) => x.Reset());
        }

        protected void Apply(System.Action<BehaviourNode> action)
        {
            foreach (var item in children) action(item);
        }
    }
}