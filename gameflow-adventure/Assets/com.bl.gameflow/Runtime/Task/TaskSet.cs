using System.Collections;
using System.Collections.Generic;

namespace GameFlow
{
    /// <summary>
    /// 任务集合：*并行*执行其中的子任务，当这些子任务全部完成时，使集合完成
    /// </summary>
    public class TaskSet : Task, IEnumerable<Task>, IEnumerable
    {
        private HashSet<Task> members = new HashSet<Task>();

        /// <summary>
        /// 返回包含子任务的个数
        /// </summary>
        public int Count { get { return members.Count; } }

        /// <summary>
        /// 返回已经完成的子任务个数
        /// </summary>
        public int completedCount { get; private set; }

        /// <summary>
        /// 向集合添加一个新任务
        /// </summary>
        public void Add(Task item)
        {
            if (item == null) return;
            members.Add(item);
        }

        public void Add(System.Action lambda) { Add(Lambda(lambda)); }

        public void Remove(Task item)
        {
            members.Remove(item);
        }

        public TaskSet() { }

        private void CheckComplete()
        {
            completedCount++;
            if (completedCount == Count) Complete();
        }

        protected override void OnPlay()
        {
            base.OnPlay();
            if (Count == 0) Complete();
            else
            {
                foreach (var item in members)
                    item.onComplete += CheckComplete;
                foreach (var item in members)
                    item.Play();
            }
        }

        protected override void OnKill()
        {
            foreach (var item in members) item.Kill();
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
            TaskSet obj = base.Copy() as TaskSet;
            obj.members = new HashSet<Task>();

            foreach (var m in members)
                obj.members.Add(m.Copy());
            return obj;
        }

        public static TaskSet operator +(TaskSet s, Task item)
        {
            s.Add(item);
            return s;
        }

        public static TaskSet operator -(TaskSet s, Task item)
        {
            s.Remove(item);
            return s;
        }
    }
}
