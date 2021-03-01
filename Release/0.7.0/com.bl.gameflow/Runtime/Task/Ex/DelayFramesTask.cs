using System.Collections;
using UnityEngine;

namespace GameFlow
{
    public sealed class DelayFramesTask : Task
    {
        public int frameCount { get; private set; }

        internal DelayFramesTask(int frameCount)
        {
            this.frameCount = frameCount;
        }

        protected override void OnPlay()
        {
            base.OnPlay();
            StartCoroutine(DelayCoroutine());
        }

        IEnumerator DelayCoroutine()
        {
            for (int i = 0; i < frameCount; i++)
                yield return new WaitForEndOfFrame();
            Complete();
        }
    }
}
