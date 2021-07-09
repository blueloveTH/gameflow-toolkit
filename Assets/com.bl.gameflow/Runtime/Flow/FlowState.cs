namespace GameFlow
{
    public sealed class FlowState
    {
        public string key { get; private set; }

        public FlowState(string key) { this.key = key; }

        public event TaskCallback onEnter, onExit;

        public TaskSet tasksOnEnter = new TaskSet();
        public TaskSet tasksOnExit = new TaskSet();

        internal void Enter()
        {
            onEnter?.Invoke();
        }

        internal void Exit()
        {
            onExit?.Invoke();
        }

        public bool Is(string key)
        {
            return this.key == key;
        }

        public bool Is(System.Enum e)
        {
            return this.key == FlowMachine.EnumToString(e);
        }
    }
}