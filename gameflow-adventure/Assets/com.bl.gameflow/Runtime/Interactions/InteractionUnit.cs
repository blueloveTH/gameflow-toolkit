using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GameFlow
{
    public abstract class InteractionUnit : MonoBehaviour
    {
        /// <summary>
        /// 置为true启动调试信息，将交互细节输出到控制台
        /// </summary>
        public static bool enableDebugInfo { get; set; } = false;

        private InteractionHeader _header;

        /// <summary>
        /// 返回本单元所属的交互头
        /// </summary>
        public InteractionHeader GetHeader()
        {
            if (_header != null) return _header;
            Transform parent = transform.parent;
            if (parent == null)
                throw new Exception("InteractionUnit should have a parent GameObject.");
            _header = parent.GetComponent<InteractionHeader>();
            if (_header == null) _header = parent.gameObject.AddComponent<InteractionHeader>();
            return _header;
        }

        /// <summary>
        /// 等价于GetHeader().GetComponent&lt;T&gt;()
        /// </summary>
        public T GetCpntInHeader<T>() where T : Component
        {
            return GetHeader().GetComponent<T>();
        }

        /// <summary>
        /// 将信号发射到目标交互单元
        /// </summary>
        protected void Emit(Signal signal, InteractionUnit target)
        {
            if (target == null) return;
            //if (signal.src.GetHeader() == target.GetHeader())
            //    return;
            target.OnSignalInternal(signal);
        }

        /// <summary>
        /// 将信号发射到目标游戏对象，其包含的所有交互单元都会收到这个信号
        /// </summary>
        protected void Emit(Signal signal, GameObject target)
        {
            var units = target.GetComponents<InteractionUnit>();
            foreach (var item in units)
            {
                if (signal.isBlocked) break;
                Emit(signal, item);
            }
        }

        /// <summary>
        /// 将信号发射到目标交互头，其所属的所有交互单元都会收到这个信号
        /// </summary>
        protected void Emit(Signal signal, InteractionHeader target)
        {
            GetHeader().Emit(signal, target);
        }

        private Dictionary<Action<Signal>, Func<Signal, bool>> slotDic = new Dictionary<Action<Signal>, Func<Signal, bool>>();

        /// <summary>
        /// 添加一个信号接收器
        /// </summary>
        /// <param name="slot">接收器函数</param>
        /// <param name="filter">过滤器函数</param>
        public void AddSlot(Action<Signal> slot, Func<Signal, bool> filter = null)
        {
            slotDic[slot] = filter;
        }

        /// <summary>
        /// 移除一个信号接收器
        /// </summary>
        public void RemoveSlot(Action<Signal> slot) { slotDic.Remove(slot); }

        internal void OnSignalInternal(Signal signal)
        {
            if (signal.isBlocked || !isActiveAndEnabled) return;
            if (!CanReceive(signal)) return;

            foreach (var item in staticSlots)
                if (item.Key.CanReceive(signal))
                {
                    if (enableDebugInfo) print(signal.Summary(this));
                    item.Value.Invoke(this, new object[] { signal });
                }

            foreach (var item in slotDic)
            {
                if (item.Value != null && !item.Value.Invoke(signal))
                    continue;
                if (enableDebugInfo) print(signal.Summary(this));
                item.Key.Invoke(signal);
            }
        }

        /// <summary>
        /// 用于过滤本单元所接收的所有信号，默认返回true
        /// </summary>
        protected virtual bool CanReceive(Signal signal)
        {
            return true;
        }


        public string unitName {
            get {
                if (transform.parent == null)
                    return string.Format("{0} ({1})", name, GetType().Name);
                else
                    return string.Format("{0}/{1} ({2})", transform.parent.name, name, GetType().Name);
            }
        }

        private Dictionary<SlotFunction, MethodInfo> _slots;
        private Dictionary<SlotFunction, MethodInfo> staticSlots {
            get {
                if (_slots == null)
                    _slots = GetSlots(GetType());
                return _slots;
            }
        }

        private static Dictionary<SlotFunction, MethodInfo> GetSlots(Type type)
        {
            Dictionary<SlotFunction, MethodInfo> slots = new Dictionary<SlotFunction, MethodInfo>();
            foreach (MethodInfo m in type.GetMethods(
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
            {
                var sfa = m.GetCustomAttribute<SlotFunction>(true);
                if (sfa != null)
                {
                    slots.Add(sfa, m);
                }
            }
            return slots;
        }

        #region Global Signals
        private HashSet<string> signals = new HashSet<string>();
        protected void AddGlobalSignal(string signalName)
        {
            if (signals.Add(signalName))
                InteractionCenter.main.AddSignalInternal(this, signalName);
        }

        protected void AddGlobalSignal(System.Enum e)
        {
            AddGlobalSignal(e.ToStringKey());
        }

        protected void RemoveGlobalSignal(string signalName)
        {
            if (signals.Remove(signalName))
                InteractionCenter.main.RemoveSignalInternal(this, signalName);
        }

        protected void RemoveGlobalSignal(System.Enum e)
        {
            RemoveGlobalSignal(e.ToStringKey());
        }

        protected void EmitGlobal(Signal signal)
        {
            if (signals.Contains(signal.name))
                InteractionCenter.main.EmitInternal(this, signal);
        }

        protected Signal Signal(string name)
        {
            return new Signal(this, name);
        }

        protected Signal Signal(System.Enum e)
        {
            return new Signal(this, e.ToStringKey());
        }
        #endregion
    }

}

internal static class EnumExtension
{
    internal static string ToStringKey(this System.Enum e)
    {
        return string.Format("{0}.{1}", e.GetType().Name, e.ToString());
    }
}