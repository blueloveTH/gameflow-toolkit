using System;
using System.Collections.Generic;

namespace GameFlow
{
    /// <summary>
    /// StandardTrie (Untested)
    /// </summary>
    public sealed class StandardTrie<T>
    {
        public sealed class Leaf
        {
            public BranchNode parent { get; private set; }

            /// <summary>
            /// Calculates the key recursively
            /// </summary>
            public string key {
                get {
                    throw new NotImplementedException();
                }
            }
            public int depth { get { return parent.depth; } }
            public T value;

            internal Leaf(T value, BranchNode parent)
            {
                this.value = value;
                this.parent = parent;
            }

            public void Delete()
            {
                parent.DeleteLeaf();
            }
        }
        public sealed class BranchNode
        {
            public int depth { get; private set; }
            public char c { get; private set; }
            public static BranchNode CreateRoot()
            {
                return new BranchNode(0, '$');
            }
            private Dictionary<char, BranchNode> children = new Dictionary<char, BranchNode>();
            private BranchNode(int depth, char c)
            {
                this.depth = depth;
                this.c = c;
            }
            public Leaf leaf { get; private set; }

            public BranchNode FindChild(char c)
            {
                if (children.ContainsKey(c)) return children[c];
                return null;
            }

            public Leaf[] FindAllLeaves()
            {
                List<Leaf> results = new List<Leaf>();
                if (leaf != null) results.Add(leaf);
                foreach (var item in children)
                    results.AddRange(item.Value.FindAllLeaves());
                return results.ToArray();
            }

            public bool DeleteChild(char c)
            {
                return children.Remove(c);
            }
            public void DeleteAllChildren()
            {
                children.Clear();
            }

            public void DeleteLeaf() { leaf = null; }
            internal void AddLeaf(T value)
            {
                if (leaf != null) throw new ArgumentException("Leaf already exists.");
                leaf = new Leaf(value, this);
            }
            public BranchNode AddChild(char c)
            {
                if (!children.ContainsKey(c))
                {
                    children.Add(c, new BranchNode(depth + 1, c));
                }
                return children[c] as BranchNode;
            }
        }

        public BranchNode root { get; private set; }

        public BranchNode FindBranch(string s)
        {
            if (string.IsNullOrEmpty(s)) return null;
            BranchNode n = root;
            int i = 0;
            for (; i < s.Length; i++)
            {
                n = n.FindChild(s[i]);
                if (n == null) return null;
            }
            return n;
        }

        public Leaf FindLeaf(string s, bool precisely)
        {
            BranchNode branch = FindBranch(s);
            if (branch == null) return null;

            if (precisely) return branch.leaf;

            var leaves = branch.FindAllLeaves();
            if (leaves.Length == 1) return leaves[0];
            return null;
        }

        public StandardTrie()
        {
            root = BranchNode.CreateRoot();
        }

        public T this[string s] {
            get {
                Leaf leaf = FindLeaf(s, true);
                if (leaf == null) throw new KeyNotFoundException();
                else return leaf.value;
            }
            set {
                Leaf leaf = FindLeaf(s, true);
                if (leaf == null) throw new KeyNotFoundException();
                else leaf.value = value;
            }
        }

        public bool TryGetValue(string key, out T value, bool precisely)
        {
            Leaf leaf = FindLeaf(key, precisely);
            if (leaf != null)
            {
                value = leaf.value;
                return true;
            }

            value = default(T);
            return false;
        }

        public bool Contains(string s, bool precisely = true)
        {
            return FindLeaf(s, precisely) != null;
        }

        public void Add(string key, T value)
        {
            if (string.IsNullOrEmpty(key)) return;
            BranchNode n = root;
            int i = 0;
            for (; i < key.Length; i++)
            {
                n = n.AddChild(key[i]);
            }
            n.AddLeaf(value);
        }
        public bool Remove(string key, bool precisely = true)
        {
            Leaf leaf = FindLeaf(key, precisely);
            if (leaf != null)
            {
                leaf.Delete();
                return true;
            }
            else return false;
        }
        public void Clear()
        {
            root.DeleteAllChildren();
        }

        public IEnumerator<T> GetEnumerator()
        {
            Leaf[] leaves = root.FindAllLeaves();
            for (int i = 0; i < leaves.Length; i++)
            {
                yield return leaves[i].value;
            }
        }
    }
}