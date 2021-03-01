using System.Collections;
using System.Collections.Generic;

namespace GameFlow
{
    public sealed class PriorityQueue<T> : IEnumerable, IEnumerable<T>
    {
        private List<T> heap = new List<T>();
        public bool IsEmpty { get { return heap.Count == 0; } }
        public int Count { get { return heap.Count; } }
        public void Clear() { heap.Clear(); }
        public void Push(T item)
        {
            heap.Add(item);
            HeapAdjustUp(heap.Count - 1);
        }

        private int property = 1;
        private IComparer<T> m_comparer;
        private void HeapAdjustUp(int index)
        {
            while (index > 0)
            {
                int parent = index >> 1;
                if ((index & 1) == 0) parent--;
                if (m_comparer.Compare(heap[index], heap[parent]) == property)
                {
                    T temp = heap[index];
                    heap[index] = heap[parent];
                    heap[parent] = temp;
                }
                else break;

                index = parent;
            }
        }

        private void HeapAdjustDown(int index)
        {
            int size = heap.Count;
            int leftChild, rightChild, biggerChild;
            while (true)
            {
                leftChild = (index << 1) + 1;
                rightChild = leftChild + 1;
                if (leftChild < size && rightChild < size)
                {
                    if (m_comparer.Compare(heap[leftChild], heap[rightChild]) == property) biggerChild = leftChild;
                    else biggerChild = rightChild;
                }
                else
                {
                    if (leftChild < size) biggerChild = leftChild;
                    else if (rightChild < size) biggerChild = rightChild;
                    else return;
                }

                if (m_comparer.Compare(heap[index], heap[biggerChild]) == -property)
                {
                    T temp = heap[index];
                    heap[index] = heap[biggerChild];
                    heap[biggerChild] = temp;
                    index = biggerChild;
                    continue;
                }

                return;
            }
        }


        public PriorityQueue(bool minHeap)
        {
            m_comparer = Comparer<T>.Default;
            if (minHeap) property = -1;
        }
        public T Top()
        {
            return heap[0];
        }
        public T Pop()
        {
            T t = Top();
            heap[0] = heap[heap.Count - 1];
            heap.RemoveAt(heap.Count - 1);
            HeapAdjustDown(0);
            return t;
        }

        public bool Contains(T item)
        {
            return heap.Contains(item);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return ((IEnumerable<T>)heap).GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
