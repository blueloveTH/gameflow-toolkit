//-----------------------------------------------------
// Proto* framework (c) 2016 Raziel Anarki
//-----------------------------------------------------

using System.Collections.Generic;

namespace Proto.AI
{
	public abstract class Composite : Routine
	{
		// an abstract routine that can have many child routines
		//-----------------------------------------------------

		protected List<Routine> routines = new List<Routine> ();

		//-----------------------------------------------------

		protected Composite ()
			: base ()
		{
		}

		protected Routine Construct (List<Routine> routines)
		{
			this.routines.AddRange (routines); // copy elements

			return Construct ();
		}

		//-----------------------------------------------------

		public override void Reset ()
		{
			routines.ForEach ((routine) => routine.Reset ());

			base.Reset ();
		}

		public override void Dispose ()
		{
			routines.ForEach ((routine) => routine.Dispose ());
			routines.Clear ();

			base.Dispose ();
		}
	}
}