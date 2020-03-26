using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFlow
{
    public class FlowDiagram : IEnumerable<FlowNode>
    {
        #region Serialization
        private Dictionary<string, FlowNode> nodes = new Dictionary<string, FlowNode>();
        #endregion

        private TaskList currentTask;
        public bool isLocked { get; set; } = false;

        public FlowNode currentNode { get; private set; }
        public FlowNode this[string name] => nodes[name];
        public event System.Action<FlowNode> onCurrentNodeChange;

        private FlowDiagram() { }

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

        public IEnumerator<FlowNode> GetEnumerator()
        {
            foreach (var item in nodes)
                yield return item.Value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator() as IEnumerator;
        }

        #region Non-public Methods

        public T AddNode<T>(string name) where T : FlowNode, new()
        {
            T newNode = new T();
            newNode.name = name;
            newNode.diagram = this;
            nodes.Add(newNode.name, newNode);
            return newNode;
        }

        public FlowNode AddNode(string name)
        {
            return AddNode<FlowNode>(name);
        }

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

        public void SaveToAsset()
        {

        }

        #region Built-in Diagrams
        public static FlowDiagram BinaryDiagram()
        {
            var fd = new FlowDiagram();
            FlowNode offNode = fd.AddNode("0");
            FlowNode onNode = fd.AddNode("1");
            onNode.AddArc(offNode);
            offNode.AddArc(onNode);
            return fd;
        }

        public static FlowDiagram Empty()
        {
            return new FlowDiagram();
        }
        #endregion
    }
}