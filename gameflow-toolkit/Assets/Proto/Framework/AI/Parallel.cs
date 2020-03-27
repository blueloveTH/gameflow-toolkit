//-----------------------------------------------------
// Proto* framework (c) 2016 Raziel Anarki
//-----------------------------------------------------

using System.Collections.Generic;


namespace Proto.AI
{
    public class Parallel : Composite
    {
        // a composite that ticks all subroutines concurrently
        // waits until all routines finish execution (by default),
        // (or)
        // waits until the fastest one finishes (and interrupts the rest)
        //-----------------------------------------------------

        public enum ExitPolicy
        {
            RequireOne,
            RequireAll
        }

        ExitPolicy policy = ExitPolicy.RequireAll;

        //-----------------------------------------------------

        protected Parallel()
            : base()
        {
        }

        public Routine Construct(ExitPolicy policy, List<Routine> routines)
        {
            this.policy = policy;

            return Construct(routines);
        }

        //-----------------------------------------------------

        static List<Routine> temp = new List<Routine>();

        public static Routine Run(params object[] args)
        {
            Parallel routine = (StaticPool.Request<Parallel>() ?? new Parallel());
            ExitPolicy policy = ExitPolicy.RequireAll;
            temp.Clear();

            foreach (object arg in args)
            {
                if (arg is ExitPolicy)
                    policy = (ExitPolicy)arg;
                else
                if (arg is Routine)
                    temp.Add((Routine)arg);
                else
                if (arg is Routine[])
                    temp.AddRange((Routine[])arg);
                else
                if (arg is List<Routine>)
                    temp.AddRange((List<Routine>)arg);
                else
                if (arg is IEnumerable<Routine>)
                    temp.Add(Subroutine.Run((IEnumerable<Routine>)arg));
            }

            //Composite.Construct() will copy elements from temp
            return routine.Construct(policy, temp);
        }

        //-----------------------------------------------------

        public override void Update()
        {
            UnityEngine.Profiling.Profiler.BeginSample(string.Format("Paralell.Run({0})", policy == ExitPolicy.RequireAll ? "RequireAll" : "RequireOne"));

            if (!running)
            {
                UnityEngine.Profiling.Profiler.EndSample();
                return;
            }

            int terminated = 0;

            for (int i = 0; i < routines.Count; i++)
            {
                routines[i].Update();

                if (!routines[i].running)
                    terminated++;
            }

            if ((policy == ExitPolicy.RequireOne && terminated > 0)
                || (policy == ExitPolicy.RequireAll && terminated == routines.Count))
                status = Status.Terminated;

            UnityEngine.Profiling.Profiler.EndSample();
        }
    }
}