using System.Collections.Generic;

namespace GameFlow
{
    public sealed class FlowArc
    {
        #region Serialization
        private Dictionary<string, object> data;

        /// <summary>
        /// 返回源节点
        /// </summary>
        public FlowNode source { get; private set; }
        /// <summary>
        /// 返回目标节点
        /// </summary>
        public FlowNode target { get; private set; }
        #endregion

        internal FlowArc(FlowNode source, FlowNode target)
        {
            if (source.diagram != target.diagram) throw new System.Exception("Diagram doesn't match");

            data = new Dictionary<string, object>();

            this.source = source;
            this.target = target;
        }

        /// <summary>
        /// 从data字典取出值
        /// </summary>
        public T GetData<T>(string key)
        {
            return (T)data[key];
        }

        /// <summary>
        /// 向data字典添加一个键值对
        /// </summary>
        public FlowArc AddData(string key, object value)
        {
            data.Add(key, value);
            return this;
        }

        /// <summary>
        /// 设置data字典中的一个键值对
        /// </summary>
        public FlowArc SetData(string key, object value)
        {
            data[key] = value;
            return this;
        }
    }
}