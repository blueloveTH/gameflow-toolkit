using System.Collections;
using System.Collections.Generic;

namespace GameFlow
{
    public class TaskSet : Task, IEnumerable<Task>, IEnumerable
    {
        private HashSet<Task> members = new HashSet<Task>();

        public int Count { get { return members.Count; } }
        public int completedCnt { get; private set; }

        public void Add(Task item)
        {
            members.Add(item);
            item.onComplete += Item_OnComplete;
        }

        public void Add(System.Action item) { Add(Task.ActionTask(item)); }

        public void Remove(Task item)
        {
            members.Remove(item);
            item.onComplete -= Item_OnComplete;
        }

        public TaskSet() { }

        private void Item_OnComplete()
        {
            completedCnt++;
            if (completedCnt == Count) Complete();
        }

        protected override void OnPlay()
        {
            base.OnPlay();
            if (Count == 0) Complete();
            foreach (var item in members) item.Play();
        }

        protected override void OnKill()
        {
            base.OnKill();
            foreach (var item in members) item.Kill();
        }

        public IEnumerator<Task> GetEnumerator()
        {
            return ((IEnumerable<Task>)members).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator() as IEnumerator;
        }
    }
}
