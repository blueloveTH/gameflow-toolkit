using System.Collections;
using UnityEngine;

namespace GameFlow
{
    public sealed class DelayTask : Task
    {
        public float duration { get; private set; }
        public bool useUnscaledTime { get; private set; }

        internal DelayTask(float duration, bool useUnscaledTime)
        {
            this.duration = duration;
            this.useUnscaledTime = useUnscaledTime;
        }

        protected override void OnPlay()
        {
            base.OnPlay();
            StartCoroutine(DelayCoroutine());
        }

        IEnumerator DelayCoroutine()
        {
            if (useUnscaledTime)
                yield return new WaitForSecondsRealtime(duration);
            else
                yield return new WaitForSeconds(duration);
            Complete();
        }

    }
}