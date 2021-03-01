using System.Collections.Generic;

namespace GameFlow
{
    public sealed class FlowArc
    {
        private Dictionary<string, dynamic> data;

        public FlowNode source { get; private set; }
        public FlowNode target { get; private set; }

        internal FlowArc(FlowNode source, FlowNode target)
        {
            if (source.owner != target.owner)
                throw new System.Exception("Inconsistent connection.");

            data = new Dictionary<string, dynamic>();

            this.source = source;
            this.target = target;

            this.source.arcList.Add(this);
        }

        public dynamic this[string key]
        {
            get { return data[key]; }
            set { data[key] = value; }
        }

        public FlowArc AddData(string key, dynamic value)
        {
            data.Add(key, value);
            return this;
        }
    }
}
