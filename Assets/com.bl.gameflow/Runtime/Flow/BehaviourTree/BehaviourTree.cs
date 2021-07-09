using System.Collections;
using UnityEngine;

namespace GameFlow.Tree
{
    public enum BehaviourStatus
    {
        Failure = -1,
        Running = 0,
        Success = 1,
    }

    public sealed class BehaviourTree : BehaviourNode
    {
        private BehaviourNode child;
        private Coroutine coroutine;
        public MonoBehaviour owner { get; private set; }
        public BehaviourStatus status { get; private set; }

        public bool loop { get; set; }
        public float tickRate { get; set; }
        public bool paused { get; set; }

        public event System.Action onComplete;

        public BehaviourTree(MonoBehaviour owner, BehaviourNode child, bool loop = true, float tickRate = 0.2f)
        {
            this.owner = owner;
            this.child = child;
            this.loop = loop;
            this.tickRate = tickRate;
        }

        public override void Reset()
        {
            paused = false;
            child.Reset();
        }

        public override BehaviourStatus Tick()
        {
            return child.Tick();
        }

        public void Play()
        {
            coroutine = owner.StartCoroutine(TreeCoroutine());
        }

        public void Stop()
        {
            owner.StopCoroutine(coroutine);
        }

        private IEnumerator TreeCoroutine()
        {
            do
            {
                while (true)
                {
                    if (paused) yield return new WaitForSeconds(tickRate);

                    status = Tick();
                    if (status != BehaviourStatus.Running) break;
                    yield return new WaitForSeconds(tickRate);
                }
                yield return new WaitForEndOfFrame();
            } while (loop);

            onComplete?.Invoke();
        }
    }
}