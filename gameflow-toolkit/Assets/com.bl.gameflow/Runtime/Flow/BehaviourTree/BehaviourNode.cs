using System;
using UnityEngine;

namespace GameFlow
{
    [DisallowMultipleComponent()]
    public abstract class BehaviourNode : MonoBehaviour
    {
        public BehaviourTree tree { get; private set; }
        protected virtual void Awake()
        {
            tree = GetComponentInParent<BehaviourTree>();
        }

        public abstract BehaviourStatus Tick();
        protected abstract void OnInit();
        internal virtual void Init()
        {
            OnInit();
        }

        private void Reset()
        {
            gameObject.name = GetType().Name;
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class NodeMenuItem : Attribute
    {
        public string path { get; private set; }
        public Type type { get; set; }
        public string description;

        public NodeMenuItem(string path)
        {
            this.path = path;
        }

        public NodeMenuItem(string path, string description)
        {
            this.path = path;
            this.description = description;
        }
    }
}