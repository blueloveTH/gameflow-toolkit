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
        public int completedCnt { get; private set; }

        /// <summary>
        /// 向集合添加一个新任务
        /// </summary>
        public void Add(Task item)
        {
            members.Add(item);
            item.onComplete += Item_OnComplete;
        }

        public void Add(System.Action item) { Add(Task.DoAction(item)); }

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
