//-----------------------------------------------------
// Proto* framework (c) 2016 Raziel Anarki
//-----------------------------------------------------

using System;
using System.Collections.Generic;

namespace Proto.AI
{
	public abstract class Decorator : Routine
	{
		// an abstract routine that has one child routine
		//-----------------------------------------------------

		protected Routine routine;

		//-----------------------------------------------------

		protected Decorator ()
			: base ()
		{
		}

		protected Routine Construct (Routine routine)
		{
			this.routine = routine;

			return Construct ();
		}

		//-----------------------------------------------------

		public override void Reset ()
		{
			routine.Reset ();

			base.Reset ();
		}

		public override void Dispose ()
		{
			routine.Dispose ();
			routine = null;

			base.Dispose ();
		}
	}
}