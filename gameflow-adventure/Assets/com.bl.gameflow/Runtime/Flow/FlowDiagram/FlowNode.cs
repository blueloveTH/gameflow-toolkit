using System;
using System.Collections.Generic;

namespace GameFlow
{
    public class FlowNode
    {
        #region Serialization
        private List<FlowArc> arcList = new List<FlowArc>();
        /// <summary>
        /// 返回节点名称
        /// </summary>
        public string name { get; internal set; }
        /// <summary>
        /// 返回节点所属的流程图
        /// </summary>
        public FlowDiagram diagram { get; internal set; }
        #endregion
        /// <summary>
        /// 返回节点包含的有向边数目
        /// </summary>
        public int arcCount => arcList.Count;
        /// <summary>
        /// 返回节点的第一条有向边
        /// </summary>
        public FlowArc firstArc => arcCount > 0 ? arcList[0] : null;

        /// <summary>
        /// 指定是否已经进入本节点
        /// </summary>
        public bool IsEntered { get; private set; }

        public FlowNode() { }

        /// <summary>
        /// 当进入本节点时，触发此事件
        /// </summary>
        public event System.Action<FlowNode> onEnter;
        /// <summary>
        /// 当退出本节点时，触发此事件
        /// </summary>
        public event System.Action<FlowNode> onExit;

        /// <summary>
        /// 进入节点时执行任务
        /// <code>enterTaskCreator += ()=>Task.DelayTask(1f); //在进入节点前等待1秒</code>
        /// </summary>
        public TaskCreator enterTaskCreator { get; set; } = new TaskCreator();
        /// <summary>
        /// 退出节点时执行任务
        /// <code>exitTaskCreator += ()=>Task.DelayTask(1f); //在退出节点前等待1秒</code>
        /// </summary>
        public TaskCreator exitTaskCreator { get; set; } = new TaskCreator();

        /// <summary>
        /// 查找符合条件的第一条有向边
        /// </summary>
        public FlowArc FindArc(Predicate<FlowArc> match)
        {
            return arcList.Find(match);
        }

        /// <summary>
        /// 返回第index条有向边
        /// </summary>
        public FlowArc GetArc(int index)
        {
            return arcList[index];
        }

        /// <summary>
        /// 创建一条由本节点指向target节点的有向边
        /// </summary>
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