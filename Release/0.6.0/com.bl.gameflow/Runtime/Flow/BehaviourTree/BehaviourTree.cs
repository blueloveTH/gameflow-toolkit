using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFlow.AI
{
    public enum BehaviourStatus
    {
        Failure = -1,
        Running = 0,
        Success = 1,
    }

    public sealed class BehaviourTree : IEnumerable<BehaviourNode>
    {
        private Dictionary<string, BehaviourNode> nodes = new Dictionary<string, BehaviourNode>();

        public bool restartWhenComplete { get; set; } = true;
        public float tickRate = 0.1f;

        public MonoBehaviour owner { get; private set; }

        public BehaviourTree(MonoBehaviour owner)
        {
            this.owner = owner;
        }

        public BehaviourNode root => nodes["root"];

        internal T CreateNode<T>(string name) where T : BehaviourNode, new()
        {
            T newNode = new T();
            newNode.name = name;
            newNode.tree = this;
            nodes.Add(name, newNode);
            return newNode;
        }

        public T CreateRoot<T>() where T : BehaviourNode, new()
        {
            var newNode = CreateNode<T>("root");
            return newNode;
        }

        private TreePlayTask treePlayTask;
        public bool IsPlaying => treePlayTask.IsPlaying();

        public void Begin()
        {
            if (IsPlaying) return;

            treePlayTask = PlayTask();
            treePlayTask.Play();
        }

        public void Reset()
        {
            foreach (var item in this)
                (item as BehaviourNode).Reset(this);
        }

        public BehaviourStatus Tick()
        {
            return root.Tick();
        }

        public void End()
        {
            treePlayTask?.Kill();
        }

        public TreePlayTask PlayTask()
        {
            var t = new TreePlayTask(this);
            if (restartWhenComplete) t.onComplete += Begin;
            return t;
        }

        public IEnumerator<BehaviourNode> GetEnumerator()
        {
            foreach (var item in nodes)
                yield return item.Value;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator() as IEnumerator;
        }

        public sealed class TreePlayTask : Task
        {
            public BehaviourTree tree { get; private set; }

            public TreePlayTask(BehaviourTree tree)
            {
                this.tree = tree;
                owner = tree.owner;
            }

            protected override void OnPlay()
            {
                tree.Reset();
                StartCoroutine(TreeCoroutine());
                base.OnPlay();
            }

            private IEnumerator TreeCoroutine()
            {
                while (true)
                {
                    if (tree.Tick() != BehaviourStatus.Running) break;
                    yield return new WaitForSeconds(tree.tickRate);
                }
                yield return new WaitForEndOfFrame();
                Complete();
            }
        }
    }
}