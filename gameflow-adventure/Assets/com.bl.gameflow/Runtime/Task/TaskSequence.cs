namespace GameFlow
{
    /// <summary>
    /// 任务序列：继承自TaskList，新增在指定时间点插入新任务的功能
    /// </summary>
    public class TaskSequence : TaskList
    {
        private TaskSet seqBody;

        public TaskSequence()
        {
            seqBody = new TaskSet();
            Add(seqBody);
        }

        /// <summary>
        /// 在指定时间点插入新任务
        /// <code>Insert(0.5f, t); //任务t在序列开始后的0.5秒后被执行</code>
        /// </summary>
        public void Insert(float atPosition, Task task)
        {
            TaskList list = new TaskList();
            list.Add(Task.Delay(atPosition));
            list.Add(task);

            seqBody.Add(list);
        }

        /// <summary>
        /// 在指定时间点插入函数回调
        /// <code>Insert(0.5f, ()=>Debug.Log("123")); //在序列开始0.5秒后打印"123"</code>
        /// </summary>
        public void Insert(float atPosition, System.Action action)
        {
            Insert(atPosition, Task.DoAction(action));
        }
    }
}
