using UnityEngine;

namespace GameFlow
{
    [DisallowMultipleComponent()]
    public sealed class InteractionHeader : MonoBehaviour
    {
        public void Emit(Signal signal, InteractionHeader target)
        {
            if (target == null || signal.src == target) return;
            target.OnSignalInternal(signal);
        }

        private void OnSignalInternal(Signal signal)
        {
            if (!isActiveAndEnabled) return;

            var units = transform.GetCpntsInDirectChildren<InteractionUnit>();
            foreach (var item in units)
            {
                if (signal.isBlocked) break;
                item.OnSignalInternal(signal);
            }
        }
    }
}
