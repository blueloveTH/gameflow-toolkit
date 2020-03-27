//-----------------------------------------------------
// Proto* framework (c) 2016 Raziel Anarki
//-----------------------------------------------------

using System;
using System.Collections.Generic;


namespace Proto.AI
{
    public class Until : Decorator
    {
        // a decorator that runs a routine in parallel until another (the "condition") stops running
        // (possibly) interrupting the routine when the condition stops
        // the routine itself may finish earlier
        // "Until" returns if and only when the condition stops running
        //-----------------------------------------------------

        Routine condition;

        //-----------------------------------------------------

        public Until()
            : base()
        {
        }

        public Routine Construct(Routine condition, Routine routine)
        {
            this.condition = condition;

            return Construct(routine);
        }

        //-----------------------------------------------------

        public static Routine Run(Func<bool> condition, Routine routine)
        {
            return (StaticPool.Request<While>() ?? new While()).Construct(condition, routine);
        }

        public static Routine Run(Func<bool> condition, IEnumerable<Routine> routine)
        {
            return (StaticPool.Request<While>() ?? new While()).Construct(condition, Subroutine.Run(routine));
        }

        //-----------------------------------------------------

        public static Routine Run(Routine condition, Routine routine)
        {
            return (StaticPool.Request<Until>() ?? new Until()).Construct(condition, routine);
        }

        public static Routine Run(Routine condition, IEnumerable<Routine> routine)
        {
            return (StaticPool.Request<Until>() ?? new Until()).Construct(condition, Subroutine.Run(routine));
        }

        public static Routine Run(IEnumerable<Routine> condition, Routine routine)
        {
            return (StaticPool.Request<Until>() ?? new Until()).Construct(Subroutine.Run(condition), routine);
        }

        public static Routine Run(IEnumerable<Routine> condition, IEnumerable<Routine> routine)
        {
            return (StaticPool.Request<Until>() ?? new Until()).Construct(Subroutine.Run(condition), Subroutine.Run(routine));
        }

        //-----------------------------------------------------

        public override void Reset()
        {
            condition.Reset();

            base.Reset();
        }

        public override void Update()
        {
            UnityEngine.Profiling.Profiler.BeginSample("Until.Run({0})");

            if (!running)
            {
                UnityEngine.Profiling.Profiler.EndSample();
                return;
            }

            condition.Update();

            if (condition.running)
                routine.Update();
            else
                status = Status.Terminated;

            UnityEngine.Profiling.Profiler.EndSample();
        }

        public override void Dispose()
        {
            if (condition != null)
            {
                condition.Dispose();
                condition = null;
            }

            base.Dispose();
        }
    }
}