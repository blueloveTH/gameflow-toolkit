//-----------------------------------------------------
// Proto* framework (c) 2016 Raziel Anarki
//-----------------------------------------------------

using System.Collections.Generic;
using Proto.AI;
using UnityEngine;

namespace Proto
{
	public class ThinkerManager : MonoBehaviour
	{
		// ticks many thinker routines from a single Unity callback
		// have this on a gameobject in your scene somewhere
		// to tick all registered thinkers
		//------------------------------------------------------

		static List<Routine> routines = new List<Routine> ();

		public static Routine Register (Routine routine)
		{
			if (!routines.Contains (routine))
				routines.Add (routine);

			return routine;
		}

		public static Routine Unregister (Routine routine)
		{
			routines.Remove (routine);

			routine.Dispose ();
			routine = null;

			return routine;
		}

		//------------------------------------------------------

		void FixedUpdate ()
		{
			int terminated = 0; 

			for (int i = 0; i < routines.Count; i++)
			{
				routines[i].Update ();

				if (!routines[i].running)
					terminated++;
			}

			if (terminated > 16 && terminated > routines.Count / 4)
				CleanUp ();
		}

		//------------------------------------------------------

		void CleanUp ()
		{
			List<Routine> running = new List<Routine> (routines.Capacity);

			for (int i = 0; i < routines.Count; i++)
			{
				if (routines[i].running)
					running.Add (routines[i]);
			}

			routines.Clear ();
			routines = running;
		}
	}
}
