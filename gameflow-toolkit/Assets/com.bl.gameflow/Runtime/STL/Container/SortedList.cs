/*
using System;
using System.Collections;
using System.Collections.Generic;

namespace STL.Container
{
    public sealed class SortedList<T> : IEnumerable
    {
        private List<T> list = new List<T>();

        private IComparer<T> m_comparer;
        public SortedList(IComparer<T> comparer = null)
        {
            if (comparer != null) m_comparer = comparer;
            else m_comparer = Comparer<T>.Default;
        }

        public bool IsEmpty { get { return list.Count == 0; } }
        public int Count { get { return list.Count; } }
        public void Clear() { list.Clear(); }

        //0 is the max,and last index is the min
        public void Insert(T item)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (m_comparer.Compare(item, list[i]) >= 0)
                {
                    list.Insert(i, item);
                    return;
                }
            }

            list.Add(item);
        }

        public T MaxItem { get { return list[0]; } }

        public bool Remove(T item)
        {
            return list.Remove(item);
        }

        public bool Contains(T item) { return list.Contains(item); }

        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)list).GetEnumerator();
        }
    }
}
*/