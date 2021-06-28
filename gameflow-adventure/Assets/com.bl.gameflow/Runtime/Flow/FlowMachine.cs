using System.Collections.Generic;

namespace GameFlow
{
    public sealed class FlowMachine<T> : FlowMachine where T : System.Enum
    {
        public FlowMachine()
        {
            foreach (var item in System.Enum.GetNames(typeof(T)))
                Add(typeof(T).Name + "." + item);
        }

        public FlowState this[T e] => states[EnumToString(e)];

        public void Enter(T e)
        {
            base.Enter(EnumToString(e));
        }
    }

    public class FlowMachine
    {
        protected readonly Dictionary<string, FlowState> states = new Dictionary<string, FlowState>();
        private TaskList currentTask;

        public event System.Action<string> onStateChange;
        public bool isLocked { get; set; } = false;
        public FlowState currentState { get; private set; }

        public FlowMachine() { }

        public FlowState this[string key]
        {
            get
            {
                if (!states.ContainsKey(key))
                    Add(key);
                return states[key];
            }
        }

        public void Add(string key)
        {
            states.Add(key, new FlowState(key));
        }

        public bool Remove(string key)
        {
            return states.Remove(key);
        }

        public void Enter(string key)
        {
            if (!states.ContainsKey(key))
                throw new KeyNotFoundException(key);
            var target = states[key];

            if (currentState == target || isLocked || currentTask.IsPlaying()) return;

            currentTask = new TaskList();

            if (currentState != null)
            {
                var oldState = currentState;
                currentTask.Add(oldState.tasksOnExit.Copy());
                currentTask.Add(oldState.Exit);
            }

            currentTask.Add(() =>
            {
                currentState = target;
                onStateChange?.Invoke(key);
            });

            currentTask.Add(target.tasksOnEnter.Copy());
            currentTask.Add(target.Enter);

            currentTask.Play();
        }

        internal static string EnumToString(System.Enum e)
        {
            return string.Format("{0}.{1}", e.GetType().Name, e.ToString());
        }
    }
}