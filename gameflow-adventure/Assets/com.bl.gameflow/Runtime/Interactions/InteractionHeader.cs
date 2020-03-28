using UnityEngine;

namespace GameFlow
{
    [DisallowMultipleComponent()]
    public sealed class InteractionHeader : MonoBehaviour
    {
        /// <summary>
        /// 将信号发射到目标交互头，其所属的所有交互单元都会收到这个信号
        /// </summary>
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
