namespace GameFlow
{
    public class TaskSequence : TaskList
    {
        private TaskSet seqBody;

        public TaskSequence()
        {
            seqBody = new TaskSet();
            Add(seqBody);
        }

        public void Insert(float atPosition, Task task)
        {
            TaskList list = new TaskList();
            list.Add(Task.DelayTask(atPosition, owner));
            list.Add(task);

            seqBody.Add(list);
        }
    }
}
