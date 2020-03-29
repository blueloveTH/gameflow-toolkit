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

            foreach (var item in GetUnits())
            {
                if (signal.isBlocked) break;
                item.OnSignalInternal(signal);
            }
        }

        /// <summary>
        /// 返回第一个指定类型的交互单元
        /// </summary>
        public T GetUnit<T>() where T : InteractionUnit
        {
            return transform.GetCpntInDirectChildren<T>();
        }

        /// <summary>
        /// 返回所有交互单元
        /// </summary>
        public InteractionUnit[] GetUnits()
        {
            return transform.GetCpntsInDirectChildren<InteractionUnit>();
        }
    }
}
