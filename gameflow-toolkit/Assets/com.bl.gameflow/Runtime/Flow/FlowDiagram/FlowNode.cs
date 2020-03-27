using System;
using System.Collections.Generic;

namespace GameFlow
{
    public class FlowNode
    {
        #region Serialization
        private List<FlowArc> arcList = new List<FlowArc>();
        public string name { get; internal set; }
        public FlowDiagram diagram { get; internal set; }
        #endregion

        public int arcCount => arcList.Count;
        public FlowArc firstArc => arcCount > 0 ? arcList[0] : null;

        public bool IsEntered { get; private set; }

        public FlowNode() { }

        public event System.Action<FlowNode> onEnter;
        public event System.Action<FlowNode> onExit;

        public TaskCreator enterTaskCreator { get; set; } = new TaskCreator();
        public TaskCreator exitTaskCreator { get; set; } = new TaskCreator();

        public FlowArc FindArc(Predicate<FlowArc> match)
        {
            return arcList.Find(match);
        }

        public FlowArc GetArc(int index)
        {
            return arcList[index];
        }

        public FlowArc AddArc(FlowNode target)
        {
            FlowArc arc = new FlowArc(this, target);
            arcList.Add(arc);
            return arc;
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

        protected internal virtual Task OnEnterTask() { return null; }
        protected internal virtual Task OnExitTask() { return null; }
        protected virtual void OnEnter() { onEnter?.Invoke(this); }
        protected virtual void OnExit() { onExit?.Invoke(this); }
    }
}