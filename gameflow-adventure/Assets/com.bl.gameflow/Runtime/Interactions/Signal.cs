﻿using System;
using System.Collections.Generic;

namespace GameFlow
{
    public sealed class Signal
    {
        /// <summary>
        /// 返回信号的名称
        /// </summary>
        public string name { get; private set; }
        /// <summary>
        /// 返回信号的源
        /// </summary>
        public InteractionUnit src { get; private set; }
        /// <summary>
        /// 指示信号是否已被屏蔽失效
        /// </summary>
        public bool isBlocked { get; private set; }
        /// <summary>
        /// 指示是否是一个全局广播信号
        /// </summary>
        public bool isGlobal { get; internal set; }

        private Dictionary<string, object> data;

        internal Signal(InteractionUnit src, string name)
        {
            if (src == null) throw new ArgumentNullException("src");
            this.name = name;
            this.src = src;
            data = new Dictionary<string, object>();
        }

        /// <summary>
        /// 向data字典添加一个键值对
        /// </summary>
        public Signal AddData(string key, object value)
        {
            data.Add(key, value);
            return this;
        }

        /// <summary>
        /// 从data字典取出值
        /// </summary>
        public T GetData<T>(string key)
        {
            return (T)data[key];
        }

        /// <summary>
        /// 将该信号置于屏蔽状态，使其失效，不能再被传递
        /// </summary>
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