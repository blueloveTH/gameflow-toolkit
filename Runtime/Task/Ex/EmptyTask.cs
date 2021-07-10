namespace GameFlow
{
    public sealed class EmptyTask : Task
    {
        internal EmptyTask() { }

        protected override void OnKill()
        {

        }

        protected override void OnPlay()
        {

        }
    }
}