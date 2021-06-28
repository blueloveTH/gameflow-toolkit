using UnityEngine.Events;

namespace GameFlow
{
    public sealed class WaitEventTask : Task
    {
        UnityEvent e;
        bool isListenerAdded = false;

        internal WaitEventTask(UnityEvent e)
        {
            this.e = e;
        }

        private void Trigger()
        {
            e.RemoveListener(Trigger);
            isListenerAdded = false;
            Complete();
        }

        protected override void OnPlay()
        {
            e.AddListener(Trigger);
            isListenerAdded = true;
        }

        protected override void OnKill()
        {
            if (isListenerAdded)
            {
                e.RemoveListener(Trigger);
                isListenerAdded = false;
            }
        }

    }
}