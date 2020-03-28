using System.Collections.Generic;

namespace STL.Container
{
    public sealed class ObjectPool<T> where T : class, IEnumerable<T>
    {
        private Stack<T> buffer = new Stack<T>();

        public Creator creator { get; private set; }
        public OnPush onPush { get; private set; }
        public OnPop onPop { get; private set; }

        public delegate T Creator();
        public delegate void OnPush(T t);
        public delegate void OnPop(T t);

        public ObjectPool(Creator creator, OnPush onPush = null, OnPop onPop = null)
        {
            this.creator = creator;
            this.onPop = onPop;
            this.onPush = onPush;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return buffer.GetEnumerator();
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
        public int Count { get { return buffer.Count; } }
    }

}
