using System.Collections.Generic;
using Proto;
using Proto.AI;
using UnityEngine;

namespace Proto.Example
{
	public class Trail : Thinker
	{
		// a bullet trail fx, dies when detached
		//------------------------------------------------------

		TrailRenderer trail;

		//------------------------------------------------------

		void Awake ()
		{
			trail = GetComponentInChildren<TrailRenderer> ();
		}

		//------------------------------------------------------

		protected override IEnumerable<Routine> Think ()
		{
			while (transform.parent != null)
				yield return null;

			yield return Subroutine.Run (Wait (trail.time));

			gameObject.SetActive (false);
		}
	}
}