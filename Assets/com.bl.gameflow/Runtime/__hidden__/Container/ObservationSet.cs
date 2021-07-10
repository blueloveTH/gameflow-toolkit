using System.Collections;
using System.Collections.Generic;

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
                onAdd?.Invoke(item);
            return successful;
        }

        public bool Remove(T item)
        {
            bool successful = currentSet.Remove(item);
            if (successful)
                onRemove?.Invoke(item);
            return successful;
        }


        public event System.Action<T> onAdd;
        public event System.Action<T> onRemove;

        public void Overwrite(HashSet<T> newSet)
        {
            var diffSet = new HashSet<T>(currentSet);
            diffSet.SymmetricExceptWith(newSet);

            foreach (var item in diffSet)
            {
                if (newSet.Contains(item))
                    onAdd?.Invoke(item);
                else
                    onRemove?.Invoke(item);
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