using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GameFlow
{
    public abstract class InteractionUnit : MonoBehaviour
    {
        public static bool enableDebugInfo { get; set; } = false;

        private InteractionHeader _header;

        public InteractionHeader header {
            get {
                if (_header == null)
                {
                    _header = transform.GetCpntInDirectParent<InteractionHeader>();
                }
                return _header;
            }
        }

        protected void Emit(Signal signal, InteractionUnit target)
        {
            if (target == null) return;
            if (target.header != null && signal.src.header == target.header)
                return;
            target.OnSignalInternal(signal);
        }

        protected void Emit(Signal signal, GameObject target)
        {
            var units = target.GetComponents<InteractionUnit>();
            foreach (var item in units)
            {
                if (signal.isBlocked) break;
                Emit(signal, item);
            }
        }

        protected void Emit(Signal signal, InteractionHeader target)
        {
            header.Emit(signal, target);
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