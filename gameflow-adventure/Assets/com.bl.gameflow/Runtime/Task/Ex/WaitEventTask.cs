using UnityEngine.Events;

namespace GameFlow
{
    public sealed class WaitEventTask : Task
    {
        UnityEvent e;

        internal WaitEventTask(UnityEvent e)
        {
            this.e = e;
            e.AddListener(Complete);
        }

        protected override void OnComplete()
        {
            e.RemoveListener(Complete);
            base.OnComplete();
        }

    }
}