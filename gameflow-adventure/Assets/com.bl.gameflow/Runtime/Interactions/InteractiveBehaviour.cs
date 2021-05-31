using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GameFlow
{
    public struct SlotInfo
    {
        public Action<Signal> slot;
        public Func<Signal, bool> filter;

        internal SlotInfo(Action<Signal> slot, Func<Signal, bool> filter)
        {
            this.slot = slot;
            this.filter = filter;
        }
    }

    [Obsolete("This class has been renamed. Use `InteractiveBehaviour` instead.")]
    public abstract class InteractionNode : InteractiveBehaviour { }

    public abstract class InteractiveBehaviour : MonoBehaviour
    {
        public bool debugMode { get; set; } = false;

        private List<SlotInfo> _slotInfos = null;
        protected List<SlotInfo> slotInfos => _slotInfos ?? (_slotInfos = GetStaticSlots());

        protected void Emit(Signal signal, InteractiveBehaviour target)
        {
            if (target == null) return;
            target.OnSignalInternal(signal);
        }

        public virtual GameObject owner => gameObject;

        public T GetCpntInOwner<T>() where T : Component
        {
            return owner.GetComponent<T>();
        }

        protected void Emit(Signal signal, InteractiveBehaviour[] targets)
        {
            foreach (var nd in targets)
            {
                if (signal.isBlocked) break;
                Emit(signal, nd);
            }
        }

        protected void Emit(Signal signal, GameObject go)
        {
            Emit(signal, go.GetComponents<InteractiveBehaviour>());
        }

        public void AddSlot(Action<Signal> slot, Func<Signal, bool> filter = null)
        {
            slotInfos.Add(new SlotInfo(slot, filter));
        }

        public bool RemoveSlot(Action<Signal> slot)
        {
            int index = slotInfos.FindIndex((si) => si.slot == slot);
            if (index >= 0)
                slotInfos.RemoveAt(index);
            return index >= 0;
        }

        internal void OnSignalInternal(Signal signal)
        {
            if (signal.isBlocked || !enabled) return;
            if (!CanReceive(signal)) return;

            foreach (var item in slotInfos)
            {
                if (item.filter != null && !item.filter(signal))
                    continue;
                if (debugMode || signal.debugMode) print(signal.Summary(this));
                item.slot(signal);
                if (signal.isBlocked) break;
            }
        }

        /// <summary>
        /// 用于过滤本节点所接收的所有信号，默认返回true
        /// </summary>
        protected virtual bool CanReceive(Signal signal)
        {
            return true;
        }

        protected Signal Signal(string name)
        {
            return new Signal(this, name);
        }

        public string unitName
        {
            get
            {
                if (transform.parent == null)
                    return string.Format("{0} ({1})", name, GetType().Name);
                else
                    return string.Format("{0}/{1} ({2})", transform.parent.name, name, GetType().Name);
            }
        }

        private List<SlotInfo> GetStaticSlots()
        {
            List<SlotInfo> infos = new List<SlotInfo>();
            foreach (MethodInfo m in GetType().GetMethods(
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                SlotMethod slm = m.GetCustomAttribute<SlotMethod>(true);
                if (slm != null)
                {
                    Action<Signal> slot = (signal) => m.Invoke(this, new object[] { signal });
                    infos.Add(new SlotInfo(slot, slm.Filter));
                }
            }
            return infos;
        }
    }

}


internal static class EnumExtension
{
    internal static string ToStringKey(this System.Enum e)
    {
        return string.Format("{0}.{1}", e.GetType().Name, e.ToString());
    }
}