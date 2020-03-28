using System.Collections.Generic;

namespace GameFlow
{
    public sealed class FlowArc
    {
        #region Serialization
        public Dictionary<string, object> data { get; private set; }

        public FlowNode source { get; private set; }
        public FlowNode target { get; private set; }
        #endregion

        internal FlowArc(FlowNode source, FlowNode target)
        {
            if (source.diagram != target.diagram) throw new System.Exception("Diagram doesn't match");

            data = new Dictionary<string, object>();

            this.source = source;
            this.target = target;
        }

        public T GetValue<T>(string key)
        {
            return (T)data[key];
        }

        public FlowArc AddKeyValue(string key, object value)
        {
            data.Add(key, value);
            return this;
        }
    }
}