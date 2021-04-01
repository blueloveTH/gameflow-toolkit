using UnityEngine.Events;

namespace GameFlow
{
    public sealed class WaitEventTask : Task
    {
        UnityEvent e;
        bool tag = false;

        internal WaitEventTask(UnityEvent e)
        {
            this.e = e;
        }

        protected override void OnPlay()
        {
            base.OnPlay();
            e.AddListener(Complete);
            tag = true;
        }

        protected override void OnComplete()
        {
            e.RemoveListener(Complete);
            tag = false;
            base.OnComplete();
        }

        protected override void OnKill()
        {
            if (tag)
            {
                e.RemoveListener(Complete);
                tag = false;
            }
            base.OnKill();
        }

    }
}