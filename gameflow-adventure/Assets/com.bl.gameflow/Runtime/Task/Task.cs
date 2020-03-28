using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFlow
{
    public delegate void TaskCallback();

    public enum TaskState
    {
        /// <summary>
        /// Task对象已就绪，可以执行
        /// </summary>
        Ready,   
        /// <summary>
        /// Task对象正在运行
        /// </summary>
        Playing,
        /// <summary>
        /// Task对象已结束
        /// </summary>
        Ended
    }

    internal sealed class TaskOwnerBehaviour : MonoBehaviour { }

    public abstract class Task
    {
        #region Mono functions
        /// <summary>
        /// Task对象的所有者，默认为全局所有
        /// </summary>
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

        /// <summary>
        /// 从Task.owner启动一个Unity协程，该协程在Task对象结束后将被自动回收
        /// </summary>
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
        /// <summary>
        /// 当Play()被调用时触发此事件
        /// </summary>
        public event TaskCallback onPlay;
        /// <summary>
        /// 当Complete()被调用时触发此事件
        /// </summary>
        public event TaskCallback onComplete;
        /// <summary>
        /// 当Kill()被调用时触发此事件
        /// </summary>
        public event TaskCallback onKill;
        /// <summary>
        /// 当Kill()或Complete()被调用时触发此事件
        /// </summary>
        public event TaskCallback onEnd;

        /// <summary>
        /// 当Task对象运行时每帧触发此事件（使用Coroutine）
        /// </summary>
        public event TaskCallback onUpdate;

        /// <summary>
        /// 返回Task对象的当前状态
        /// </summary>
        public TaskState state { get; private set; } = TaskState.Ready;

        /// <summary>
        /// 指示Task对象是否完成，如果以Kill()方式结束则为false
        /// </summary>
        public bool IsCompleted { get; private set; }

        /// <summary>
        /// 执行Task对象
        /// </summary>
        public void Play()
        {
            if (state != TaskState.Ready) return;
            state = TaskState.Playing;

            if (onUpdate != null) StartCoroutine(HandleUpdateCallback());

            OnPlay();
        }

        /// <summary>
        /// 终止Task对象的执行
        /// </summary>
        public void Kill()
        {
            if (state == TaskState.Ended && !IsCompleted) return;
            state = TaskState.Ended;
            StopAllCoroutines();

            OnKill();
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
        /// 将正在运行的任务置为完成状态
        /// </summary>
        protected void Complete()
        {
            if (state != TaskState.Playing) return;
            state = TaskState.Ended;
            StopAllCoroutines();

            IsCompleted = true;

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
        public static DelayTask DelayTask(float duration, bool useUnscaledTime = false)
        {
            return new DelayTask(duration, useUnscaledTime);
        }

        /// <summary>
        /// 创建任务：等待直到给定的bool表达式为true
        /// </summary>
        public static WaitUntilTask WaitUntilTask(System.Func<bool> predicate)
        {
            return new WaitUntilTask(predicate);
        }

        /// <summary>
        /// 创建任务：调用一个无参方法（将普通函数转换为Task对象）
        /// </summary>
        public static ActionTask ActionTask(System.Action action)
        {
            return new ActionTask(action);
        }

        /// <summary>
        /// 创建任务：延迟指定帧数
        /// </summary>
        public static DelayFramesTask DelayFramesTask(int frameCount)
        {
            return new DelayFramesTask(frameCount);
        }

        public static WWWTask WWWTask(string url)
        {
            return new WWWTask(url);
        }
        #endregion
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

        /// <summary>
        /// 设置Task对象所属的MonoBehaviour
        /// </summary>
        public static T SetOwner<T>(this T task, MonoBehaviour owner) where T : Task
        {
            task.owner = owner;
            return task;
        }

        /// <summary>
        /// 为Task对象增加onComplete回调（链式风格）
        /// </summary>
        public static T OnComplete<T>(this T task, TaskCallback callback) where T : Task
        {
            task.onComplete += callback;
            return task;
        }

        /// <summary>
        /// 为Task对象增加onPlay回调（链式风格）
        /// </summary>
        public static T OnPlay<T>(this T task, TaskCallback callback) where T : Task
        {
            task.onPlay += callback;
            return task;
        }

        /// <summary>
        /// 为Task对象增加onKill回调（链式风格）
        /// </summary>
        public static T OnKill<T>(this T task, TaskCallback callback) where T : Task
        {
            task.onKill += callback;
            return task;
        }

        /// <summary>
        /// 为Task对象增加onEnd回调（链式风格）
        /// </summary>
        public static T OnEnd<T>(this T task, TaskCallback callback) where T : Task
        {
            task.onEnd += callback;
            return task;
        }
    }

}

