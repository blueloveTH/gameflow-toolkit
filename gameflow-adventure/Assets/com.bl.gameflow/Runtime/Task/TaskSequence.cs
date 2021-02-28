namespace GameFlow
{
    /// <summary>
    /// 任务序列：通过时间轴插入任务
    /// </summary>
    public class TaskSequence : TaskSet
    {
        private bool useUnscaledTime;

        public TaskSequence(bool useUnscaledTime = false)
        {
            this.useUnscaledTime = useUnscaledTime;
        }

        /// <summary>
        /// 在指定时间点插入新任务
        /// <code>Insert(0.5f, t); //任务t在序列开始后的0.5秒后被执行</code>
        /// </summary>
        public void Insert(float atTime, Task task)
        {
            TaskList list = new TaskList();
            list.Add(Delay(atTime, useUnscaledTime));
            list.Add(task);

            Add(list);
        }

        /// <summary>
        /// 在指定时间点插入无参数Lambda表达式
        /// <code>Insert(0.5f, ()=>Debug.Log("123")); //在序列开始0.5秒后打印"123"</code>
        /// </summary>
        public void Insert(float atTime, System.Action lambda)
        {
            Insert(atTime, Lambda(lambda));
        }
    }
}
