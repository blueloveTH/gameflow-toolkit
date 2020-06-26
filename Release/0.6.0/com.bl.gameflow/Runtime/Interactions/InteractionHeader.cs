using UnityEngine;

namespace GameFlow
{
    [DisallowMultipleComponent()]
    public sealed class InteractionHeader : MonoBehaviour
    {
        /// <summary>
        /// 将信号发射到目标交互头，其所属的所有交互节点都会收到这个信号
        /// </summary>
        public void Emit(Signal signal, InteractionHeader target)
        {
            if (target == null || signal.src == target) return;
            target.OnSignalInternal(signal);
        }

        private void OnSignalInternal(Signal signal)
        {
            if (!isActiveAndEnabled) return;

            foreach (var item in GetNodes())
            {
                if (signal.isBlocked) break;
                item.OnSignalInternal(signal);
            }
        }

        /// <summary>
        /// 返回第一个指定类型的交互节点
        /// </summary>
        public T GetNode<T>() where T : InteractionNode
        {
            return transform.GetCpntInDirectChildren<T>();
        }

        /// <summary>
        /// 返回所有交互节点
        /// </summary>
        public InteractionNode[] GetNodes()
        {
            return transform.GetCpntsInDirectChildren<InteractionNode>();
        }
    }
}
