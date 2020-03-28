using System;

namespace GameFlow
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class SlotFunction : Attribute
    {
        private string signalName;

        public SlotFunction(string signalName)
        {
            this.signalName = signalName;
        }

        public SlotFunction()
        {

        }

        internal bool CanReceive(Signal signal)
        {
            if (string.IsNullOrEmpty(signalName)) return true;
            return signal.name == signalName;
        }
    }
}