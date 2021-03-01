using System;
using System.Collections.Generic;

namespace GameFlow
{
    public class FlowNode
    {
        internal readonly List<FlowArc> arcList = new List<FlowArc>();

        public string name { get; internal set; }
        public FlowMachine owner { get; internal set; }

        public int arcCount => arcList.Count;
        public FlowArc firstArc => arcCount > 0 ? arcList[0] : null;
        public bool IsEntered { get; private set; }

        public FlowNode() { }

        public event Action<FlowNode> onEnter;
        public event Action<FlowNode> onExit;

        public TaskSet tasksOnEnter = new TaskSet();
        public TaskSet tasksOnExit = new TaskSet();

        public FlowArc FindArc(Predicate<FlowArc> match)
        {
            return arcList.Find(match);
        }

        public bool CompareName(string name)
        {
            return this.name == name;
        }

        public bool CompareName(System.Enum e)
        {
            return this.name == e.ToStringKey();
        }

        internal void Enter()
        {
            IsEntered = true;
            OnEnter();
        }

        internal void Exit()
        {
            IsEntered = false;
            OnExit();
        }

        protected virtual void OnEnter() { onEnter?.Invoke(this); }
        protected virtual void OnExit() { onExit?.Invoke(this); }
    }
}