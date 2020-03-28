using System.Collections;
using System.Collections.Generic;

namespace GameFlow
{
    public class TaskList : Task, IEnumerable<Task>, IEnumerable
    {
        private int index = -1;
        private List<Task> members = new List<Task>();

        public int Count { get { return members.Count; } }
        public Task current {
            get {
                if (index == -1) return null;
                else return members[index];
            }
        }

        public TaskList() { }

        public void Add(Task item)
        {
            if (item == null) return;
            members.Add(item);
            item.onComplete += Item_OnComplete;
        }

        public void Add(System.Action item) { Add(Task.ActionTask(item)); }

        public void InsertAt(int index, Task item)
        {
            if (item == null) return;
            members.Insert(index, item);
            item.onComplete += Item_OnComplete;
        }

        public void InsertAt(int index, System.Action item) { InsertAt(index, Task.ActionTask(item)); }

        public void RemoveAt(int index)
        {
            members[index].onComplete -= Item_OnComplete;
            members.RemoveAt(index);
        }
        public bool Remove(Task item)
        {
            int i = members.FindIndex((x) => x == item);
            if (i == -1) return false;
            else
            {
                RemoveAt(i);
                return true;
            }
        }

        private void Item_OnComplete()
        {
            NextItem();
        }

        private void NextItem()
        {
            index++;
            if (index < members.Count)
                members[index].Play();
            else Complete();
        }

        protected override void OnPlay()
        {
            base.OnPlay();
            index = -1;
            NextItem();
        }

        protected override void OnKill()
        {
            base.OnKill();
            if (index < Count && current != null) current.Kill();
        }

        public IEnumerator<Task> GetEnumerator()
        {
            return ((IEnumerable<Task>)members).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<Task>)members).GetEnumerator();
        }
    }
}