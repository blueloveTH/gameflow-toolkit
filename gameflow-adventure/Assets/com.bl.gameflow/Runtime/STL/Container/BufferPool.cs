using System;
using System.Collections.Generic;

namespace STL.Container
{
    public sealed class BufferPool<T> where T : class
    {
        private Stack<T> buffer = new Stack<T>();

        Creator creator;
        OnPush onPush;
        OnPop onPop;

        public delegate T Creator();
        public delegate void OnPush(T t);
        public delegate void OnPop(T t);

        public BufferPool(Creator creator, OnPush onPush, OnPop onPop)
        {
            SetDelegates(creator, onPush, onPop);
        }

        public BufferPool()
        {

        }

        public void SetDelegates(Creator creator, OnPush onPush, OnPop onPop)
        {
            this.creator = creator;
            this.onPush = onPush;
            this.onPop = onPop;
        }

        public void Push(T t)
        {
            onPush(t);
            buffer.Push(t);
        }

        public T Pop()
        {
            T t;
            if (Count == 0) t = creator();
            else t = buffer.Pop();
            onPop(t);
            return t;
        }

        public void Clear() { buffer.Clear(); }
        public int Count { get { return buffer.Count; } }
    }

}
