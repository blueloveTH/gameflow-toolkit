using System.Collections.Generic;
using Proto;
using Proto.AI;
using UnityEngine;

namespace Proto.Example
{
	public class Hit : Thinker
	{
		// a hit fx (also used as the muzzle flash)
		//------------------------------------------------------

		public float lifeTime = 1f;

		protected override IEnumerable<Routine> Think ()
		{
			yield return Subroutine.Run (Wait (lifeTime));

			gameObject.SetActive (false);
		}
	}
}