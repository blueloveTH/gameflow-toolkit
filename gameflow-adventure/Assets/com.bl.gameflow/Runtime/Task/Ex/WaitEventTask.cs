using System;
using System.Reflection;
using System.Linq.Expressions;

namespace GameFlow
{
    public sealed class WaitEventTask : Task
    {
        private EventInfo eventInfo;
        private Delegate eventHandler;
        private System.Func<object> objGetter;
        private object obj;

        internal WaitEventTask(System.Func<object> objGetter, System.Type type, string eventName)
        {
            this.objGetter = objGetter;

            eventInfo = type.GetEvent(eventName,
                BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);

            int cnt = eventInfo.EventHandlerType.GenericTypeArguments.Length;

            switch (cnt)
            {
                case 0: eventHandler = (System.Action)(() => Complete()); break;
                case 1: eventHandler = (System.Action<object>)((_1) => Complete()); break;
                case 2: eventHandler = (System.Action<object, object>)((_1, _2) => Complete()); break;
                case 3: eventHandler = (System.Action<object, object, object>)((_1, _2, _3) => Complete()); break;
                default:
                    throw new Exception("Too many parameters. (MAX=3)");
            }
        }

        protected override void OnPlay()
        {
            base.OnPlay();
            obj = objGetter();
            eventInfo.AddEventHandler(obj, eventHandler);
        }

        protected override void OnComplete()
        {
            base.OnComplete();
            if (obj != null) eventInfo.RemoveEventHandler(obj, eventHandler);
        }

        protected override void OnKill()
        {
            base.OnKill();
            if (obj != null) eventInfo.RemoveEventHandler(obj, eventHandler);
        }
    }
}
