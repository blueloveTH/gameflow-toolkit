﻿using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GameFlow
{
    public class SlotInfo
    {
        public Action<Signal> slot;
        public Func<Signal, bool> filter;

        internal SlotInfo(Action<Signal> slot, Func<Signal, bool> filter)
        {
            this.slot = slot;
            this.filter = filter;
        }
    }

    public abstract class InteractionNode : MonoBehaviour
    {
        public bool debugMode { get; set; } = false;

        private List<SlotInfo> _slotInfos = null;
        protected List<SlotInfo> slotInfos => _slotInfos ?? (_slotInfos = GetStaticSlots());

        protected void Emit(Signal signal, InteractionNode target)
        {
            if (target == null) return;
            target.OnSignalInternal(signal);
        }

        protected void Emit(Signal signal, InteractionNode[] targets)
        {
            foreach (var nd in targets)
            {
                if (signal.isBlocked) break;
                Emit(signal, nd);
            }
        }

        protected void Emit(Signal signal, GameObject go)
        {
            Emit(signal, go.GetComponents<InteractionNode>());
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
            if (signal.isBlocked || !isActiveAndEnabled) return;
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

        protected Signal Signal(System.Enum e)
        {
            return new Signal(this, e.ToStringKey());
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
                    Action<Signal> slot = (signal) => SendMessage(m.Name, signal, SendMessageOptions.RequireReceiver);
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