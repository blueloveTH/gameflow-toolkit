namespace GameFlow.AI
{
    public abstract class BehaviourNode
    {
        public BehaviourTree tree { get; internal set; }
        public string name { get; internal set; }

        internal abstract BehaviourStatus Tick();
        internal virtual void Reset(BehaviourTree tree)
        {

        }

    }
}