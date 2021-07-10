using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFlow
{
    public delegate void TaskCallback();

    public enum TaskState
    {
        Ready,
        Playing,
        Completed,
        Killed,
    }

    internal sealed class TaskOwnerBehaviour : MonoBehaviour { }

    public abstract class Task
    {
        #region Mono functions

        public MonoBehaviour owner { get; internal set; }
        private List<Coroutine> coroutines = new List<Coroutine>();

        public Task()
        {
            owner = GetDefaultOwner();
        }

        private static TaskOwnerBehaviour _defaultOwner;
        private static MonoBehaviour GetDefaultOwner()
        {
            if (_defaultOwner == null)
            {
                GameObject obj = new GameObject("Task Owner (Default)");
                obj.hideFlags = HideFlags.HideAndDontSave;
                UnityEngine.Object.DontDestroyOnLoad(obj);
                _defaultOwner = obj.AddComponent<TaskOwnerBehaviour>();
            }
            return _defaultOwner;
        }

        protected Coroutine StartCoroutine(IEnumerator routine)
        {
            var coroutine = owner.StartCoroutine(routine);
            if (coroutine != null) coroutines.Add(coroutine);
            return coroutine;
        }

        protected void StopAllCoroutines()
        {
            coroutines.ForEach((c) => owner.StopCoroutine(c));
            coroutines.Clear();
        }

        protected void StopCoroutine(Coroutine c)
        {
            if (coroutines.Remove(c))
                owner.StopCoroutine(c);
        }

        #endregion

        #region Lifecycle
        public event TaskCallback onPlay, onComplete, onKill, onEnd;
        public TaskState state { get; private set; } = TaskState.Ready;

        public void Play()
        {
            if (state != TaskState.Ready) return;
            state = TaskState.Playing;

            OnPlay();
            onPlay?.Invoke();
        }

        public void Kill()
        {
            if (state == TaskState.Killed) return;
            StopAllCoroutines();
            state = TaskState.Killed;

            OnKill();
            onKill?.Invoke();
            onEnd?.Invoke();
        }

        protected void Complete()
        {
            if (state != TaskState.Playing) return;
            StopAllCoroutines();
            state = TaskState.Completed;

            onComplete?.Invoke();
            onEnd?.Invoke();
        }

        protected abstract void OnPlay();
        protected abstract void OnKill();

        #endregion

        public virtual Task Copy()
        {
            if (state != TaskState.Ready)
                throw new System.Exception("Cannot copy a task not at ready state.");
            var task = MemberwiseClone() as Task;
            task.coroutines = new List<Coroutine>();
            return task;
        }


        #region Built-in task

        public static DelayTask Delay(float duration, bool useUnscaledTime = false)
        {
            return new DelayTask(duration, useUnscaledTime);
        }

        public static ProgressDelayTask ProgressDelay(float duration, System.Action<float> onProgress, bool useUnscaledTime = false)
        {
            return new ProgressDelayTask(duration, useUnscaledTime, onProgress);
        }

        public static WaitUntilTask WaitUntil(System.Func<bool> predicate)
        {
            return new WaitUntilTask(predicate);
        }

        public static LambdaTask Lambda(System.Action lambda)
        {
            return new LambdaTask(lambda);
        }

        public static FunctionTask<T> Function<T>()
        {
            return new FunctionTask<T>();
        }

        public static DelayFramesTask DelayFrames(int frameCount)
        {
            return new DelayFramesTask(frameCount);
        }

        public static WaitEventTask WaitEvent(UnityEngine.Events.UnityEvent e)
        {
            return new WaitEventTask(e);
        }

        public static EmptyTask Empty()
        {
            return new EmptyTask();
        }

        #endregion
    }
}

