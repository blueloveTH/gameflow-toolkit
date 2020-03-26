using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFlow
{
    public abstract class ParentNode : BehaviourNode
    {
        /// <summary>
        /// Identical to GetChild(0) with null check
        /// </summary>
        public BehaviourNode child0 {
            get {
                if (childCount == 0) return null;
                return GetChild(0);
            }
        }

        protected BehaviourNode GetChild(int i)
        {
            return transform.GetChild(i).GetComponent<BehaviourNode>();
        }

        protected int childCount { get { return transform.childCount; } }

        internal override void Init()
        {
            base.Init();
            ForEachChild((x) => x.Init());
        }

        protected BehaviourNode[] GetChildNodes()
        {
            return transform.GetCpntsInDirectChildren<BehaviourNode>();
        }

        protected void ForEachChild(System.Action<BehaviourNode> action)
        {
            int cnt = childCount;
            for (int i = 0; i < cnt; i++)
            {
                action.Invoke(GetChild(i));
            }
        }
    }
}