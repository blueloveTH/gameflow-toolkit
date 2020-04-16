using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameFlow
{
    internal sealed class InteractionCenter
    {
        internal Dictionary<string, MetaSignal> metaDic = new Dictionary<string, MetaSignal>();

        private static InteractionCenter _main;
        public static InteractionCenter main => _main ?? (_main = new InteractionCenter());

        internal void AddSignalInternal(InteractionUnit src, string signalName)
        {
            metaDic.Add(signalName, new MetaSignal(signalName, src));
        }

        internal void RemoveSignalInternal(InteractionUnit src, string signalName)
        {
            if (metaDic[signalName].src != src)
                throw new InvalidOperationException("A signal can only be operatored by its owner.");
            metaDic.Remove(signalName);
        }

        internal void EmitInternal(InteractionUnit src, Signal signal)
        {
            var signalName = signal.name;
            var metaSlots = metaDic[signalName];

            if (metaSlots.src != src)
                throw new InvalidOperationException("A signal can only be operatored by its owner.");

            //signal.isGlobal = true;
            HashSet<MetaSlot> nullSlots = new HashSet<MetaSlot>();
            foreach (var s in metaSlots.mSlots)
            {
                if (s.target == null)
                {
                    nullSlots.Add(s);
                    continue;
                }
                s.target.SendMessage(s.methodName, signal, SendMessageOptions.RequireReceiver);
            }

            metaSlots.mSlots.RemoveAll((x) => nullSlots.Contains(x));
        }
    }

    public static class InteractionCenterExtension
    {
        private static InteractionCenter center => InteractionCenter.main;

        public static void Connect(this MonoBehaviour target, string signalName, string slotName)
        {
            MetaSignal metaSignal;
            if (center.metaDic.TryGetValue(signalName, out metaSignal))
                metaSignal.mSlots.Add(new MetaSlot(target, slotName));
            else
                Debug.LogWarning("No such signal: " + signalName + ".");
        }

        public static void Disconnect(this MonoBehaviour target, string signalName, string slotName)
        {
            MetaSignal metaSignal;
            if (center.metaDic.TryGetValue(signalName, out metaSignal))
                metaSignal.mSlots.Remove(new MetaSlot(target, slotName));
            else
                Debug.LogWarning("No such signal: " + signalName + ".");
        }
    }
}