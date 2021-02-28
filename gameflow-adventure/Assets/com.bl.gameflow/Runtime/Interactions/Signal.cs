using System;
using System.Collections.Generic;

namespace GameFlow
{
    public sealed class Signal
    {
        public string name { get; private set; }
        public InteractionNode source { get; private set; }
        public bool isBlocked { get; private set; }
        public bool debugMode { get; set; } = false;

        private Dictionary<string, object> data;

        internal Signal(InteractionNode source, string name)
        {
            if (source == null) throw new ArgumentNullException("source");
            this.name = name;
            this.source = source;
            data = new Dictionary<string, object>();

            isBlocked = false;
        }

        public object this[string key]
        {
            get { return data[key]; }
            set { data[key] = value; }
        }

        public Signal AddData(string key, object value)
        {
            data.Add(key, value);
            return this;
        }

        public Signal Debug()
        {
            debugMode = true;
            return this;
        }

        public void Block()
        {
            isBlocked = true;
        }

        public bool CompareName(string name)
        {
            return this.name == name;
        }

        public bool CompareName(System.Enum e)
        {
            return this.name == e.ToStringKey();
        }

        internal string Summary(InteractionNode target)
        {
            string txt = string.Empty;
            txt += "[SIGNAL] " + name + "\n";
            txt += source.unitName + " => " + target.unitName + "\n";
            foreach (var item in data)
                txt += ">> " + item.Key + ": " + item.Value + "\n";
            return txt;
        }
    }
}