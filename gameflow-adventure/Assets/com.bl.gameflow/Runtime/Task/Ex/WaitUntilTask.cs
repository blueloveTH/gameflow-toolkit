using UnityEngine;
using System.Collections;

namespace GameFlow
{
    public sealed class WaitUntilTask : Task
    {
        internal WaitUntilTask(System.Func<bool> predicate)
        {
            this.predicate = predicate;
        }

        private System.Func<bool> predicate;

        protected override void OnPlay()
        {
            StartCoroutine(WaitUntil());
        }

        IEnumerator WaitUntil()
        {
            yield return new WaitUntil(predicate);
            Complete();
        }

        protected override void OnKill()
        {

        }
    }
}
