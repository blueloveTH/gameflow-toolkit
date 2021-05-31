using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFlow
{
    public sealed class ObservationSet<T> : IEnumerable<T>
    {
        private HashSet<T> currentSet = new HashSet<T>();

        public int Count => currentSet.Count;

        public bool Contains(T item)
        {
            return currentSet.Contains(item);
        }

        public bool Add(T item)
        {
            bool successful = currentSet.Add(item);
            if (successful)
                OnAdd?.Invoke(item);
            return successful;
        }

        public bool Remove(T item)
        {
            bool successful = currentSet.Remove(item);
            if (successful)
                OnRemove?.Invoke(item);
            return successful;
        }


        public event System.Action<T> OnAdd;
        public event System.Action<T> OnRemove;

        public void Overwrite(HashSet<T> newSet)
        {
            var diffSet = new HashSet<T>(currentSet);
            diffSet.SymmetricExceptWith(newSet);

            foreach (var item in diffSet)
            {
                if (newSet.Contains(item))
                    OnAdd?.Invoke(item);
                else
                    OnRemove?.Invoke(item);
            }
            currentSet = newSet;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)currentSet).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)currentSet).GetEnumerator();
        }
    }


}