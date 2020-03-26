using System.Collections.Generic;
using System;
using UnityEngine;

namespace GameFlow
{
    public sealed class Signal
    {
        public string name { get; private set; }
        public Dictionary<string, object> data { get; private set; }
        public bool isBlocked { get; private set; }
        public bool isGlobal { get; internal set; }

        public InteractionUnit src { get; private set; }

        internal Signal(InteractionUnit src, string name)
        {
            if (src == null) throw new ArgumentNullException("src");
            this.name = name;
            this.src = src;
            data = new Dictionary<string, object>();
        }

        public Signal AddKeyValue(string key, object value)
        {
            data.Add(key, value);
            return this;
        }

        public void Block()
        {
            isBlocked = true;
        }

        internal string Summary(InteractionUnit target)
        {
            string txt = string.Empty;
            txt += "[SIGNAL] " + name + "\n";
            txt += src.unitName + " => " + target.unitName + "\n";
            txt += "isBlocked: " + isBlocked.ToString() + "\n";
            txt += "isGlobal: " + isGlobal.ToString() + "\n";
            txt += "data: \n";
            foreach (var item in data)
                txt += string.Format("({0}, {1})\n", item.Key, item.Value);
            return txt;
        }
    }
}