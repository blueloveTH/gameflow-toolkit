namespace GameFlow
{
    public sealed class FunctionTask<T> : Task
    {
        public T returnValue { get; private set; }

        public void Return(T value)
        {
            this.returnValue = value;
            Complete();
        }

        internal FunctionTask() { }
    }
}