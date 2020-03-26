using UnityEngine;

namespace GameFlow.Patterns
{
    public abstract class Card : ScriptableObject
    {
        public Task currentTask { get; private set; }
        public event TaskCallback onTaskComplete;

        protected abstract Task CreateExecTask();

        private void Awake()
        {
            
        }

        public Task ExecTask()
        {
            return currentTask = CreateExecTask().OnCompleteAdd(() => onTaskComplete?.Invoke());
        }

        public static T New<T>() where T : Card
        {
            return CreateInstance<T>();
        }

        public static Card New(System.Type type)
        {
            return CreateInstance(type) as Card;
        }

        public static Card New(string className)
        {
            return CreateInstance(className) as Card;
        }

        public static T NewFromRes<T>(string path) where T : Card
        {
            var src = Resources.Load<T>(path);
            return Instantiate(src);
        }

        public static Card Clone(Card card)
        {
            return Instantiate(card);
        }

    }

}