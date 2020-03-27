using System.Collections.Generic;

namespace GameFlow.AI
{
    public abstract class ParentNode : BehaviourNode
    {
        private List<BehaviourNode> children = new List<BehaviourNode>();

        public BehaviourNode child0 {
            get {
                if (childCount == 0) return null;
                return GetChild(0);
            }
        }

        public T AddChild<T>() where T : BehaviourNode, new()
        {
            string childName = string.Format("{1}/{0}: {2}", childCount, name, typeof(T).Name);
            T node = tree.CreateNode<T>(childName);
            children.Add(node);
            return node;
        }

        protected BehaviourNode GetChild(int i)
        {
            return children[i];
        }

        protected int childCount => children.Count;

        internal override void Reset(BehaviourTree tree)
        {
            base.Reset(tree);
            ForEachChild((x) => x.Reset(tree));
        }

        protected BehaviourNode[] GetChildNodes()
        {
            List<BehaviourNode> nodes = new List<BehaviourNode>();
            ForEachChild((x) => nodes.Add(x));
            return nodes.ToArray();
        }

        protected void ForEachChild(System.Action<BehaviourNode> action)
        {
            children.ForEach(action);
        }
    }
}