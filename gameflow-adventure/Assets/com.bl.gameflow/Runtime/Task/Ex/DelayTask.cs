using System.Collections;
using UnityEngine;

namespace GameFlow
{
    public sealed class ProgressDelayTask : DelayTask
    {
        internal ProgressDelayTask(float duration, bool useUnscaledTime) : base(duration, useUnscaledTime) { }

        public event System.Action<float> onUpdate;

        protected override IEnumerator DelayCoroutine()
        {
            float totalTime = duration;

            while (totalTime > 0)
            {
                onUpdate?.Invoke(1 - totalTime / duration);

                yield return new WaitForEndOfFrame();

                if (useUnscaledTime)
                    totalTime -= Time.unscaledDeltaTime;
                else
                    totalTime -= Time.deltaTime;
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
            base.OnPlay();
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

    }
}