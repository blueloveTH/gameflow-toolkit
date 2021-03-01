using System;

namespace GameFlow
{
    public sealed class EventProperty<T>
    {
        public T value { get; private set; }
        public event System.Action onValueChange;

        public EventProperty(T value)
        {
            this.value = value;
        }

        public void SetValue(T value)
        {
            if (this.value.Equals(value)) return;
            this.value = value;
            onValueChange?.Invoke();
        }

        public static implicit operator T(EventProperty<T> prop)
        {
            return prop.value;
        }
    }

    public class PropertyObserver<T>
    {
        public T oldValue { get; private set; }
        public T value { get; private set; }

        /// <summary>
        /// arg1: oldValue, arg2: newValue
        /// </summary>
        public event System.Action<T, T> onValueChange;

        private Func<T> getter;

        public PropertyObserver(Func<T> getter)
        {
            this.getter = getter;
            oldValue = value = getter();
        }

        public bool Update()
        {
            value = getter();
            if (!value.Equals(oldValue))
            {
                onValueChange?.Invoke(oldValue, value);
                oldValue = value;
                return true;
            }
            return false;
        }
    }

}