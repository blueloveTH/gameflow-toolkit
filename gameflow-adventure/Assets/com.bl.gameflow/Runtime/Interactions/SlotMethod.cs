using System;
using System.Text.RegularExpressions;

namespace GameFlow
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class SlotMethod : Attribute
    {
        private string signalName;
        private bool useRegEx;

        public SlotMethod(string signalName, bool useRegEx = false)
        {
            this.signalName = signalName;
            this.useRegEx = useRegEx;
        }

        internal bool Filter(Signal signal)
        {
            if (useRegEx)
                return Regex.IsMatch(signal.name, signalName);
            else
                return signal.name == signalName;
        }
    }
}