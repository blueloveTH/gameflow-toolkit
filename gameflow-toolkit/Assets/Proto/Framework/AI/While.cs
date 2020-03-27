//-----------------------------------------------------
// Proto* framework (c) 2016 Raziel Anarki
//-----------------------------------------------------

using System;
using System.Collections.Generic;


namespace Proto.AI
{
	public class While : Decorator
	{
		// a decorator that runs a routine while a condition callback stays true
		// (possibly) interrupting the routine when the condtion becomes false
		// the routine itself may finish earlier
		// "While" returns if and only when the callback becomes false
		//-----------------------------------------------------

		Func<bool> condition;

		//-----------------------------------------------------

		public While ()
			: base ()
		{
		}

		public Routine Construct (Func<bool> condition, Routine routine)
		{
			this.condition = condition;

			return Construct (routine);
		}

		//-----------------------------------------------------

		public static Routine Run (Func<bool> condition, Routine routine)
		{
			return (StaticPool.Request<While> () ?? new While ()).Construct (condition, routine);
		}

		public static Routine Run (Func<bool> condition, IEnumerable<Routine> routine)
		{
			return (StaticPool.Request<While> () ?? new While ()).Construct (condition, Subroutine.Run (routine));
		}

		//-----------------------------------------------------

		public static Routine Run (Routine condition, Routine routine)
		{
			return (StaticPool.Request<Until> () ?? new Until ()).Construct (condition, routine);
		}

		public static Routine Run (Routine condition, IEnumerable<Routine> routine)
		{
			return (StaticPool.Request<Until> () ?? new Until ()).Construct (condition, Subroutine.Run (routine));
		}

		public static Routine Run (IEnumerable<Routine> condition, Routine routine)
		{
			return (StaticPool.Request<Until> () ?? new Until ()).Construct (Subroutine.Run (condition), routine);
		}

		public static Routine Run (IEnumerable<Routine> condition, IEnumerable<Routine> routine)
		{
			return (StaticPool.Request<Until> () ?? new Until ()).Construct (Subroutine.Run (condition), Subroutine.Run (routine));
		}

		//-----------------------------------------------------

		public override void Update ()
		{
			UnityEngine.Profiling.Profiler.BeginSample ("While.Run()");

			if (!running)
			{
				UnityEngine.Profiling.Profiler.EndSample ();
				return;
			}

			if (condition ())
				routine.Update ();
			else
				status = Status.Terminated;

			UnityEngine.Profiling.Profiler.EndSample ();
		}

		public override void Dispose ()
		{
			condition = null;

			base.Dispose ();
		}
	}
}