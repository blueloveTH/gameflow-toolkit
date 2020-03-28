using System;
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
        Ended
    }

    internal sealed class TaskOwnerBehaviour : MonoBehaviour { }

    public abstract class Task
    {
        #region Mono functions
        public MonoBehaviour owner { get; internal set; }
        public Task()
        {
            owner = defaultOwner;
        }

        private static TaskOwnerBehaviour _defaultOwner;
        internal static MonoBehaviour defaultOwner {
            get {
                if (_defaultOwner == null)
                {
                    GameObject obj = new GameObject("Task Owner (Default)");
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    UnityEngine.Object.DontDestroyOnLoad(obj);
                    _defaultOwner = obj.AddComponent<TaskOwnerBehaviour>();
                }
                return _defaultOwner;
            }
        }

        List<Coroutine> coroutines = new List<Coroutine>();

        protected void StartCoroutine(IEnumerator routine)
        {
            coroutines.Add(owner.StartCoroutine(routine));
        }

        private void StopAllCoroutines()
        {
            foreach (var item in coroutines)
                if (item != null) owner.StopCoroutine(item);
            coroutines.Clear();
        }

        #endregion


        #region Lifecycle
        public event TaskCallback onPlay;
        public event TaskCallback onComplete;
        public event TaskCallback onKill;
        public event TaskCallback onEnd;

        /// <summary>
        /// Use coroutine (Experimental)
        /// </summary>
        public event TaskCallback onUpdate;

        public TaskState state { get; private set; } = TaskState.Ready;
        public bool IsCompleted { get; private set; }

        public void Play()
        {
            if (state != TaskState.Ready) return;
            state = TaskState.Playing;

            if (onUpdate != null) StartCoroutine(HandleUpdateCallback());

            OnPlay();
        }

        private IEnumerator HandleUpdateCallback()
        {
            while (true)
            {
                if (onUpdate != null)
                    onUpdate.Invoke();
                else
                    break;
                yield return new WaitForEndOfFrame();
            }
        }

        /// <summary>
        /// Complete a playing task.
        /// </summary>
        protected void Complete()
        {
            if (state != TaskState.Playing) return;
            state = TaskState.Ended;
            StopAllCoroutines();

            IsCompleted = true;

            OnComplete();
        }

        public void Kill()
        {
            if (state == TaskState.Ended && !IsCompleted) return;
            state = TaskState.Ended;
            StopAllCoroutines();

            OnKill();
        }


        protected virtual void OnPlay()
        {
            onPlay?.Invoke();
        }

        protected virtual void OnComplete()
        {
            onComplete?.Invoke();
            onEnd?.Invoke();
        }

        protected virtual void OnKill()
        {
            onKill?.Invoke();
            onEnd?.Invoke();
        }

        #endregion


        #region Built-in task
        public static DelayTask DelayTask(float duration, bool useUnscaledTime = false)
        {
            return new DelayTask(duration, useUnscaledTime);
        }

        public static WWWTask WWWTask(string url)
        {
            return new WWWTask(url);
        }

        public static WaitUntilTask WaitUntilTask(System.Func<bool> predicate)
        {
            return new WaitUntilTask(predicate);
        }

        public static ActionTask ActionTask(System.Action action)
        {
            return new ActionTask(action);
        }

        public static DelayFramesTask DelayFramesTask(int frameCount)
        {
            return new DelayFramesTask(frameCount);
        }
        #endregion
    }

    public static class TaskEx
    {
        public static bool IsReady(this Task task)
        {
            if (task == null) return false;
            return task.state == TaskState.Ready;
        }

        public static bool IsPlaying(this Task task)
        {
            if (task == null) return false;
            return task.state == TaskState.Playing;
        }

        public static T SetOwner<T>(this T task, MonoBehaviour owner) where T : Task
        {
            task.owner = owner;
            return task;
        }

        public static T OnCompleteAdd<T>(this T task, TaskCallback callback) where T : Task
        {
            task.onComplete += callback;
            return task;
        }

        public static T OnPlayAdd<T>(this T task, TaskCallback callback) where T : Task
        {
            task.onPlay += callback;
            return task;
        }

        public static T OnKillAdd<T>(this T task, TaskCallback callback) where T : Task
        {
            task.onKill += callback;
            return task;
        }
    }

}

