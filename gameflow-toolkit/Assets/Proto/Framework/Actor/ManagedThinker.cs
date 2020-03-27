//-----------------------------------------------------
// Proto* framework (c) 2016 Raziel Anarki
//-----------------------------------------------------

using System.Collections.Generic;
using Proto.AI;
using UnityEngine;

namespace Proto
{
	public abstract class ManagedThinker : MonoBehaviour
	{
		// monobehaviour baseclass with a managed thinker
		// does the same thing as a Thinker, minus the FixedUpdate()
		//------------------------------------------------------

		Routine routine;

		//-----------------------------------------------------

		protected virtual void OnEnable ()
		{
			routine = ThinkerManager.Register (Subroutine.Run (Think ()));
		}

		protected virtual void OnDisable ()
		{
			routine = ThinkerManager.Unregister (routine);
		}

		//-----------------------------------------------------
		// override this in subclasses

		protected virtual IEnumerable<Routine> Think ()
		{
			yield break;
		}

		//-----------------------------------------------------

		protected IEnumerable<Routine> Wait (float seconds)
		{
			float start = Time.time;

			while (Time.time - start < seconds)
				yield return null;
		}
	}
}