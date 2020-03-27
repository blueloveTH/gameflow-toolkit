//-----------------------------------------------------
// Proto* framework (c) 2016 Raziel Anarki
//-----------------------------------------------------

using System.Collections.Generic;


namespace Proto.AI
{
    public class Subroutine : Routine
    {
        // the coroutine runner node
        // this handles the ticking of coroutine iterators
        // and nesting the yielded routine nodes
        //-----------------------------------------------------

        IEnumerable<Routine> iterator;
        IEnumerator<Routine> enumerator;

        Routine routine;

        string nicename;

        //-----------------------------------------------------

        protected Subroutine()
            : base()
        {
        }

        public Routine Construct(IEnumerable<Routine> iterator)
        {
            this.iterator = iterator;

            if (UnityEngine.Profiling.Profiler.enabled)
                setNicename();

            return Construct();
        }

        //-----------------------------------------------------

        public static Routine Run(IEnumerable<Routine> iterator)
        {
            return (StaticPool.Request<Subroutine>() ?? new Subroutine()).Construct(iterator);
        }

        //-----------------------------------------------------

        public override void Reset()
        {
            DisposeRoutine();
            DisposeEnumerator();

            enumerator = iterator.GetEnumerator();

            base.Reset();
        }

        public override void Update()
        {
            UnityEngine.Profiling.Profiler.BeginSample(string.Format("Subroutine.Run({0})", nicename));

            if (!running)
            {
                UnityEngine.Profiling.Profiler.EndSample();
                return;
            }

            if (routine != null)
            {
                routine.Update();

                if (routine.running)
                {
                    UnityEngine.Profiling.Profiler.EndSample();
                    return;
                }

                DisposeRoutine();
                //fallthru // tick outer routine on the same frame
            }

            if (!enumerator.MoveNext())
            {
                status = Status.Terminated;
                UnityEngine.Profiling.Profiler.EndSample();
                return;
            }

            Routine yielded = enumerator.Current;

            if (yielded != null)
            {
                routine = yielded;
                UnityEngine.Profiling.Profiler.EndSample();
                Update(); // tick into routine on the same frame
                return;
            }

            UnityEngine.Profiling.Profiler.EndSample();
        }

        //-----------------------------------------------------

        public override void Dispose()
        {
            DisposeRoutine();
            DisposeEnumerator();

            iterator = null;

            base.Dispose();
        }

        void DisposeRoutine()
        {
            if (routine != null)
            {
                routine.Dispose();
                routine = null;
            }
        }

        void DisposeEnumerator()
        {
            if (enumerator != null)
            {
                enumerator.Dispose();
                enumerator = null;
            }
        }

        //-----------------------------------------------------

        static char[] splits = new char[] { '+', '<', '>' };

        void setNicename()
        {
            UnityEngine.Profiling.Profiler.BeginSample("getNiceName()");
            string[] parts = iterator.ToString()
                    .Split(splits, System.StringSplitOptions.RemoveEmptyEntries);

            string[] names = parts[0].Split('.');

            nicename = string.Format
            (
                "{0}.{1}()", names[names.Length - 1], parts[1]
            );
            UnityEngine.Profiling.Profiler.EndSample();
        }
    }
}
