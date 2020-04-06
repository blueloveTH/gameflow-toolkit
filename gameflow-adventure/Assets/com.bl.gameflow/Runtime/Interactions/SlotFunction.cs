using System;

namespace GameFlow
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SlotFunction : Attribute
    {
        private string signalName;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="signalName">接收的信号名称</param>
        public SlotFunction(string signalName)
        {
            this.signalName = signalName;
        }

        internal bool CanReceive(Signal signal)
        {
            if (string.IsNullOrEmpty(signalName)) return true;
            return signal.name == signalName;
        }
    }
}