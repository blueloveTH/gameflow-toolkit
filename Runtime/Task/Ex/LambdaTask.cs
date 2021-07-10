namespace GameFlow
{
    public sealed class LambdaTask : Task
    {
        private System.Action lambda;

        internal LambdaTask(System.Action lambda)
        {
            this.lambda = lambda;
        }

        protected override void OnKill()
        {

        }

        protected override void OnPlay()
        {
            lambda.Invoke();
            Complete();
        }
    }
}