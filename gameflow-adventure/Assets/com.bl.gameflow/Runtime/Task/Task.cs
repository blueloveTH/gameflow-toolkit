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
        public Task()
        {
            owner = defaultOwner;
        }

        private static TaskOwnerBehaviour _defaultOwner;
        internal static MonoBehaviour defaultOwner
        {
            get
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
        }

        private List<Coroutine> coroutines = new List<Coroutine>();

        /// <summary>
        /// 从Task.owner启动一个Unity协程，该协程在Task对象结束后将被自动回收
        /// </summary>
        protected Coroutine StartCoroutine(IEnumerator routine)
        {
            var coroutine = owner.StartCoroutine(routine);
            if (coroutine != null) coroutines.Add(coroutine);
            return coroutine;
        }

        private void StopAllCoroutines()
        {
            coroutines.ForEach((c) => owner.StopCoroutine(c));
            coroutines.Clear();
        }

        #endregion

        #region Lifecycle
        public event TaskCallback onPlay;
        public event TaskCallback onComplete;
        public event TaskCallback onKill;
        public event TaskCallback onEnd;

        public TaskState state { get; private set; } = TaskState.Ready;
        public bool IsCompleted => state == TaskState.Completed;
        public bool IsEnded => state == TaskState.Completed || state == TaskState.Killed;

        public void Play()
        {
            if (state != TaskState.Ready) return;
            state = TaskState.Playing;
            OnPlay();
        }

        public void Kill()
        {
            if (state == TaskState.Killed) return;
            StopAllCoroutines();
            state = TaskState.Killed;

            OnKill();
        }

        protected void Complete()
        {
            if (state != TaskState.Playing) return;
            StopAllCoroutines();
            state = TaskState.Completed;

            OnComplete();
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
        /// <summary>
        /// 创建任务：延迟指定时间（秒）
        /// </summary>
        public static DelayTask Delay(float duration, bool useUnscaledTime = false)
        {
            return new DelayTask(duration, useUnscaledTime);
        }

        /// <summary>
        /// 创建任务：延迟指定时间（秒），并提供进度记录。
        /// </summary>
        public static ProgressDelayTask ProgressDelay(float duration, bool useUnscaledTime = false)
        {
            return new ProgressDelayTask(duration, useUnscaledTime);
        }

        /// <summary>
        /// 创建任务：等待直到给定的bool表达式为true
        /// </summary>
        public static WaitUntilTask WaitUntil(System.Func<bool> predicate)
        {
            return new WaitUntilTask(predicate);
        }

        /// <summary>
        /// 创建任务：调用无参数Lambda表达式
        /// </summary>
        public static LambdaTask Lambda(System.Action lambda)
        {
            return new LambdaTask(lambda);
        }

        /// <summary>
        /// 创建任务：异步等待函数的返回值
        /// </summary>
        public static FunctionTask<T> Function<T>()
        {
            return new FunctionTask<T>();
        }

        /// <summary>
        /// 创建任务：延迟指定帧数
        /// </summary>
        public static DelayFramesTask DelayFrames(int frameCount)
        {
            return new DelayFramesTask(frameCount);
        }

        /// <summary>
        /// 创建任务：等待事件的发生
        /// </summary>
        public static WaitEventTask WaitEvent(UnityEngine.Events.UnityEvent e)
        {
            return new WaitEventTask(e);
        }

        //public static EmptyTask Empty()
        //{
        //    return new EmptyTask();
        //}

        #endregion

        public virtual Task Copy()
        {
            if (state != TaskState.Ready)
                throw new System.Exception("Cannot copy a task not at ready state.");
            var task = MemberwiseClone() as Task;
            task.coroutines = new List<Coroutine>();
            return task;
        }
    }

    public static class TaskEx
    {
        /// <summary>
        /// 检查Task对象是否已准备好，对null返回false
        /// </summary>
        public static bool IsReady(this Task task)
        {
            if (task == null) return false;
            return task.state == TaskState.Ready;
        }

        /// <summary>
        /// 检查Task对象是否正在运行，对null返回false
        /// </summary>
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
        public static T OnComplete<T>(this T task, TaskCallback callback) where T : Task
        {
            task.onComplete += callback;
            return task;
        }
        public static T OnPlay<T>(this T task, TaskCallback callback) where T : Task
        {
            task.onPlay += callback;
            return task;
        }
        public static T OnKill<T>(this T task, TaskCallback callback) where T : Task
        {
            task.onKill += callback;
            return task;
        }
        public static T OnEnd<T>(this T task, TaskCallback callback) where T : Task
        {
            task.onEnd += callback;
            return task;
        }
    }

}

