//-----------------------------------------------------
// Proto* framework (c) 2016 Raziel Anarki
//-----------------------------------------------------

using System;

namespace Proto.AI
{
    public abstract class Routine : IDisposable
    {
        // the abstract routine node
        // a "behavior node" used as an iterator "yield instruction"
        //-----------------------------------------------------

        public enum Status
        {
            Invalid,
            Running,
            Terminated
        }

        public Status status { get; protected set; }

        public bool running { get { return status == Status.Running; } }

        //-----------------------------------------------------

        protected Routine()
        {
            status = Status.Invalid;
        }

        protected Routine Construct()
        {
            Reset();

            return this;
        }

        //-----------------------------------------------------

        public virtual void Reset()
        {
            status = Status.Running;
        }

        public abstract void Update();

        public virtual void Dispose()
        {
            status = Status.Invalid;
            StaticPool.Return(this);
        }
    }
}
