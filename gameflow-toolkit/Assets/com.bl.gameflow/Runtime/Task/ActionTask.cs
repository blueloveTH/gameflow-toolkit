namespace GameFlow
{
    public sealed class ActionTask : Task
    {
        private System.Action action;

        internal ActionTask(System.Action action)
        {
            this.action = action;
        }

        protected override void OnPlay()
        {
            base.OnPlay();
            action.Invoke();
            Complete();
        }
    }
}