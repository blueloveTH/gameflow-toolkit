using System;

namespace GameFlow
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class SlotMethod : Attribute
    {
        private string signalName;
        private bool asPrefix;

        public SlotMethod(string signalName, bool asPrefix = true)
        {
            this.signalName = signalName;
            this.asPrefix = asPrefix;
        }

        internal bool Filter(Signal signal)
        {
            if (asPrefix)
                return signal.name.StartsWith(signalName);
            else
                return signal.name == signalName;
        }
    }
}