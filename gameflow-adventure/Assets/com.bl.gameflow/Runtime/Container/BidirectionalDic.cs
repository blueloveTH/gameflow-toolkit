using System.Collections;
using System.Collections.Generic;

namespace GameFlow
{
    public sealed class BidirectionalDic<TypeA, TypeB> : IEnumerable<KeyValuePair<TypeA, TypeB>>, IEnumerable
    {
        private Dictionary<TypeA, TypeB> dicA2B = new Dictionary<TypeA, TypeB>();
        private Dictionary<TypeB, TypeA> dicB2A = new Dictionary<TypeB, TypeA>();

        public void Add(TypeA a, TypeB b)
        {
            dicA2B.Add(a, b);
            dicB2A.Add(b, a);
        }

        public bool ContainsA(TypeA a)
        {
            return dicA2B.ContainsKey(a);
        }

        public bool ContainsB(TypeB b)
        {
            return dicB2A.ContainsKey(b);
        }

        public bool RemoveA(TypeA a)
        {
            if (!dicA2B.ContainsKey(a)) return false;
            dicB2A.Remove(dicA2B[a]);
            dicA2B.Remove(a);
            return true;
        }

        public bool RemoveB(TypeB b)
        {
            if (!dicB2A.ContainsKey(b)) return false;
            dicA2B.Remove(dicB2A[b]);
            dicB2A.Remove(b);
            return true;
        }

        public TypeB A2B(TypeA a) { return dicA2B[a]; }
        public TypeA B2A(TypeB b) { return dicB2A[b]; }

        public int Count => dicA2B.Count;

        public void Clear()
        {
            dicA2B.Clear();
            dicB2A.Clear();
        }

        public IEnumerator<KeyValuePair<TypeA, TypeB>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TypeA, TypeB>>)dicA2B).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
