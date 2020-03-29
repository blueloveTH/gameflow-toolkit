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
            if (signal.src.GetHeader() == target.GetHeader())
                return;
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

        internal void OnSignalInternal(Signal signal)
        {
            if (signal.isBlocked || !isActiveAndEnabled) return;

            foreach (var item in slots)
            {
                if (CanReceive(signal) && item.Key.CanReceive(signal))
                {
                    if (enableDebugInfo) print(signal.Summary(this));
                    item.Value.Invoke(this, new object[] { signal });
                }
            }
        }

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
        private Dictionary<SlotFunction, MethodInfo> slots {
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

        protected void RemoveGlobalSignal(string signalName)
        {
            if (signals.Remove(signalName))
                InteractionCenter.main.RemoveSignalInternal(this, signalName);
        }

        protected void Emit(Signal globalSignal)
        {
            if (signals.Contains(globalSignal.name))
                InteractionCenter.main.EmitInternal(this, globalSignal);
        }

        protected Signal Signal(string name)
        {
            return new Signal(this, name);
        }
        #endregion
    }

}