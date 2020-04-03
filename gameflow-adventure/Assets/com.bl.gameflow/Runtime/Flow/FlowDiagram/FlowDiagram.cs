using System;
using System.Collections;
using System.Collections.Generic;

namespace GameFlow
{
    /// <summary>
    /// 流程图：具有状态节点和有向边
    /// </summary>
    public sealed class FlowDiagram : IEnumerable<FlowNode>
    {
        private Dictionary<string, FlowNode> nodes = new Dictionary<string, FlowNode>();
        private TaskList currentTask;

        /// <summary>
        /// 按名称索引节点
        /// </summary>
        public FlowNode this[string name] {
            get {
                FlowNode value;
                if (nodes.TryGetValue(name, out value))
                    return value;
                return null;
            }
        }

        public FlowNode this[System.Enum e] => this[e.ToStringKey()];

        /// <summary>
        /// 锁定标记，置为true时禁止切换节点
        /// </summary>
        public bool isLocked { get; set; } = false;
        /// <summary>
        /// 当前节点改变时，触发此事件
        /// </summary>
        public event System.Action<FlowNode> onCurrentNodeChange;
        /// <summary>
        /// 返回当前节点
        /// </summary>
        public FlowNode currentNode { get; private set; }

        public FlowDiagram() { }

        /// <summary>
        /// 退出当前节点，进入新状态节点
        /// </summary>
        public void Enter(string name)
        {
            if (!nodes.ContainsKey(name)) throw new Exception("Target doesn't exist.");
            InternalChange(nodes[name]);
        }

        /// <summary>
        /// 退出当前节点，进入新状态节点
        /// </summary>
        public void Enter(FlowNode target)
        {
            if (target.diagram != this) throw new Exception("Diagram doesn't match.");
            InternalChange(target);
        }

        /// <summary>
        /// 查找当前节点的第一条有向边，进入所指向下一节点
        /// </summary>
        public void EnterNext()
        {
            if (currentNode == null) return;
            Enter(currentNode.firstArc?.target);
        }

        /// <summary>
        /// 创建一个新节点，并添加到流程图中
        /// </summary>
        public FlowNode CreateNode(string name)
        {
            var newNode = new FlowNode();
            newNode.name = name;

            AddNodeInternal(newNode);
            return newNode;
        }

        #region Non-Public Methods
        private void InternalChange(FlowNode target)
        {
            if (isLocked) return;
            if (currentNode == target) return;
            if (currentTask.IsPlaying()) return;

            TaskList list = new TaskList();

            if (currentNode != null)
            {
                var oldNode = currentNode;
                list.Add(oldNode.OnExitTask());
                list.Add(oldNode.exitTaskCreator.Invoke());
                list.Add(oldNode.Exit);
            }

            list.Add(() =>
            {
                currentNode = target;
                onCurrentNodeChange?.Invoke(target);
            });

            list.Add(target.OnExitTask());
            list.Add(target.enterTaskCreator.Invoke());
            list.Add(target.Enter);

            currentTask = list;
            list.Play();
        }

        public IEnumerator<FlowNode> GetEnumerator()
        {
            foreach (var item in nodes)
                yield return item.Value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator() as IEnumerator;
        }

        internal void AddNodeInternal(FlowNode node)
        {
            node.diagram = this;
            nodes.Add(node.name, node);
        }
        #endregion

        #region Built-in Diagrams
        /// <summary>
        /// 创建二值图，该图等价于开关
        /// </summary>
        public static FlowDiagram BinaryDiagram()
        {
            var fd = new FlowDiagram();
            FlowNode offNode = fd.CreateNode("0");
            FlowNode onNode = fd.CreateNode("1");
            onNode.AddArc(offNode);
            offNode.AddArc(onNode);
            return fd;
        }

        /// <summary>
        /// 使用Enum类型创建流程图
        /// </summary>
        public static FlowDiagram CreateByEnum<T>() where T : System.Enum
        {
            FlowDiagram fd = new FlowDiagram();
            foreach (var item in System.Enum.GetNames(typeof(T)))
                fd.CreateNode(typeof(T).Name + "." + item);
            return fd;
        }
        #endregion
    }
}