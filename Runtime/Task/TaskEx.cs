using UnityEngine;

namespace GameFlow
{
    public static class TaskEx
    {
        /// <summary>
        /// Check if the task is ready. (FALSE for null pointer)
        /// </summary>
        public static bool IsReady(this Task task)
        {
            if (task == null) return false;
            return task.state == TaskState.Ready;
        }

        /// <summary>
        /// Check if the task is playing. (FALSE for null pointer)
        /// </summary>
        public static bool IsPlaying(this Task task)
        {
            if (task == null) return false;
            return task.state == TaskState.Playing;
        }

        public static bool IsComplete(this Task task)
        {
            if (task == null) throw new System.ArgumentNullException("task");
            return task.state == TaskState.Completed;
        }

        public static bool IsEnded(this Task task)
        {
            if (task == null) throw new System.ArgumentNullException("task");
            return task.state == TaskState.Killed || task.state == TaskState.Completed;
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