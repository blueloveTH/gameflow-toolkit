using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFlow
{
    public sealed class FlowMachine : IEnumerable<FlowNode>
    {
        private readonly Dictionary<string, FlowNode> nodes = new Dictionary<string, FlowNode>();
        private int _lockCount = 0;
        private int lockCount => _lockCount + Convert.ToInt32(isLocked);

        public FlowNode this[string name] => nodes[name];
        public FlowNode this[System.Enum e] => nodes[e.ToStringKey()];

        public FlowArc Connect(FlowNode src, FlowNode dest)
        {
            return new FlowArc(src, dest);
        }
        public FlowArc Connect(string src, string dest)
        {
            return Connect(this[src], this[dest]);
        }
        public FlowArc Connect(System.Enum src, System.Enum dest)
        {
            return Connect(this[src], this[dest]);
        }

        public bool isLocked { get; set; } = false;
        public event System.Action<FlowNode> onNodeChange;
        public FlowNode currentNode { get; private set; }

        public FlowMachine() { }

        public void Enter(string key)
        {
            Enter(this[key]);
        }
        public void Enter(System.Enum key)
        {
            Enter(this[key]);
        }
        public void Enter(FlowNode target)
        {
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
            newNode.owner = this;
            nodes.Add(name, newNode);
            return newNode;
        }

        #region Non-Public Methods
        private void InternalChange(FlowNode target)
        {
            if (target.owner != this)
                throw new Exception("Inconsistent node change.");
            if (currentNode == target) return;
            if (lockCount > 0) return;
            _lockCount++;

            TaskList list = new TaskList();

            if (currentNode != null)
            {
                var oldNode = currentNode;
                list.Add(oldNode.tasksOnExit.Copy());
                list.Add(oldNode.Exit);
            }

            list.Add(() =>
            {
                currentNode = target;
                onNodeChange?.Invoke(target);
            });

            list.Add(target.tasksOnEnter.Copy());
            list.Add(target.Enter);

            list.OnComplete(() => _lockCount--);
            list.Play();
        }

        public IEnumerator<FlowNode> GetEnumerator()
        {
            foreach (var item in nodes)
                yield return item.Value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region Built-in Diagrams
        public static FlowMachine BinaryDiagram()
        {
            var fd = new FlowMachine();
            fd.CreateNode("0");
            fd.CreateNode("1");
            fd.Connect("0", "1");
            fd.Connect("1", "0");
            return fd;
        }

        public static FlowMachine FromEnum<T>() where T : System.Enum
        {
            FlowMachine fd = new FlowMachine();
            foreach (var item in System.Enum.GetNames(typeof(T)))
                fd.CreateNode(typeof(T).Name + "." + item);
            return fd;
        }
        #endregion
    }
}