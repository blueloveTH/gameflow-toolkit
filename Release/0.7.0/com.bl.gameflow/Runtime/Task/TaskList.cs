using System.Collections;
using System.Collections.Generic;

namespace GameFlow
{
    /// <summary>
    /// 任务列表：*顺序*执行其中的子任务，当这些子任务全部完成时，使列表完成
    /// </summary>
    public class TaskList : Task, IEnumerable<Task>, IEnumerable
    {
        private int index = -1;
        private List<Task> members = new List<Task>();

        public int Count { get { return members.Count; } }
        public Task current
        {
            get
            {
                if (index == -1) return null;
                else return members[index];
            }
        }

        public int currentIndex => index;

        public TaskList() { }

        public void Add(Task item)
        {
            if (item == null) return;
            members.Add(item);
        }

        public void Add(System.Action lambda)
        {
            Add(Lambda(lambda));
        }

        public bool Remove(Task item)
        {
            int i = members.FindIndex((x) => x == item);
            if (i >= 0) members.RemoveAt(i);
            return i >= 0;
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

            foreach (var item in members)
                item.onComplete += NextItem;
            NextItem();
        }

        protected override void OnKill()
        {
            if (index < Count && current != null) current.Kill();
            base.OnKill();
        }

        public IEnumerator<Task> GetEnumerator()
        {
            return ((IEnumerable<Task>)members).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override Task Copy()
        {
            TaskList obj = base.Copy() as TaskList;
            obj.members = new List<Task>();

            foreach (var m in members)
                obj.members.Add(m.Copy());
            return obj;
        }

        public static TaskList operator +(TaskList s, Task item)
        {
            s.Add(item);
            return s;
        }

        public static TaskList operator -(TaskList s, Task item)
        {
            s.Remove(item);
            return s;
        }
    }
}