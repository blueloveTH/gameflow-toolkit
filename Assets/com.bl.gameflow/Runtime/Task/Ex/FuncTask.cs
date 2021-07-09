namespace GameFlow
{
    public sealed class FuncTask<T> : Task
    {
        public T returnValue { get; private set; }

        public void Return(T value)
        {
            this.returnValue = value;
            Complete();
        }

        protected override void OnPlay()
        {

        }

        protected override void OnKill()
        {

        }

        internal FuncTask() { }
    }
}