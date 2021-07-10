using System.Collections;
using System.Collections.Generic;

namespace GameFlow
{
    public sealed class ObjectPool<T> : IEnumerable<T> where T : class
    {
        private Stack<T> buffer = new Stack<T>();

        public System.Func<T> creator { get; private set; }
        public System.Action<T> onPush { get; private set; }
        public System.Action<T> onPop { get; private set; }

        public ObjectPool(System.Func<T> creator, System.Action<T> onPush = null, System.Action<T> onPop = null)
        {
            this.creator = creator;
            this.onPop = onPop;
            this.onPush = onPush;
        }

        public void Push(T t)
        {
            onPush?.Invoke(t);
            buffer.Push(t);
        }

        public T Pop()
        {
            T t;
            if (Count == 0) t = creator();
            else t = buffer.Pop();
            onPop?.Invoke(t);
            return t;
        }

        public void Clear() { buffer.Clear(); }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)buffer).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public int Count { get { return buffer.Count; } }
    }

}
