using System.Collections;
using UnityEngine;

namespace GameFlow
{
    public sealed class ProgressDelayTask : DelayTask
    {
        internal ProgressDelayTask(float duration, bool useUnscaledTime, System.Action<float> onProgress) : base(duration, useUnscaledTime)
        {
            this.onProgress = onProgress;
        }

        private System.Action<float> onProgress;

        protected override IEnumerator DelayCoroutine()
        {
            float remainingTime = duration;

            while (remainingTime > 0)
            {
                onProgress.Invoke(1 - remainingTime / duration);

                yield return new WaitForEndOfFrame();

                if (useUnscaledTime)
                    remainingTime -= Time.unscaledDeltaTime;
                else
                    remainingTime -= Time.deltaTime;
            }
            Complete();
        }
    }

    public class DelayTask : Task
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
            StartCoroutine(DelayCoroutine());
        }

        protected virtual IEnumerator DelayCoroutine()
        {
            if (useUnscaledTime)
                yield return new WaitForSecondsRealtime(duration);
            else
                yield return new WaitForSeconds(duration);
            Complete();
        }

        protected override void OnKill()
        {

        }
    }
}