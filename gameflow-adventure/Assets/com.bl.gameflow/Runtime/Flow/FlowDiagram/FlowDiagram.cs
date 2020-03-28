using System;
using System.Collections;
using System.Collections.Generic;

namespace GameFlow
{
    public sealed class FlowDiagram : IEnumerable<FlowNode>
    {
        private Dictionary<string, FlowNode> nodes = new Dictionary<string, FlowNode>();

        public FlowNode this[string name] => nodes[name];

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

        private TaskList currentTask;
        public bool isLocked { get; set; } = false;

        public FlowNode currentNode { get; private set; }

        public event System.Action<FlowNode> onCurrentNodeChange;

        public FlowDiagram() { }

        public void Enter(string name)
        {
            if (!nodes.ContainsKey(name)) throw new Exception("Target doesn't exist.");
            InternalChange(nodes[name]);
        }

        public void Enter(FlowNode target)
        {
            if (target.diagram != this) throw new Exception("Diagram doesn't match.");
            InternalChange(target);
        }

        public void EnterNext()
        {
            if (currentNode == null) return;
            Enter(currentNode.firstArc?.target);
        }

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
        #endregion

        #region Built-in Diagrams
        public static FlowDiagram BinaryDiagram()
        {
            var fd = new FlowDiagram();
            FlowNode offNode = fd.CreateNode("0");
            FlowNode onNode = fd.CreateNode("1");
            onNode.AddArc(offNode);
            offNode.AddArc(onNode);
            return fd;
        }
        #endregion
    }
}