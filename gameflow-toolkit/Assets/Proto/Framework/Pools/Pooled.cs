//-----------------------------------------------------
// Proto* framework (c) 2016 Raziel Anarki
//-----------------------------------------------------

#define NICE_POOLS

using System.Collections.Generic;
using UnityEngine;

namespace Proto
{
	public class Pooled : MonoBehaviour
	{
		// component added to pooled instances
		//-----------------------------------------------------

		public PoolManager.Pool pool { get; set; }

		//-----------------------------------------------------

		bool wasDestroyed;

		//-----------------------------------------------------

		void OnDisable ()
		{
			if (!wasDestroyed && pool != null)
			{
				pool.Return (gameObject);

				#if NICE_POOLS
				// we cannot reparent a transform while it's being disabled
				// so we do it on the next frame instead
				Invoke ("Return", 0f);
				#endif
			}
		}
		//-----------------------------------------------------

		protected void OnDestroy ()
		{
			wasDestroyed = true;
		}

		//-----------------------------------------------------

		#if NICE_POOLS
		static Transform root;
		static Dictionary<PoolManager.Pool,Transform> holders = new Dictionary<PoolManager.Pool,Transform> ();

		void Awake ()
		{
			if (root == null)
				root = new GameObject ("PoolManager").transform;

			Transform holder;
			holders.TryGetValue (pool, out holder);

			if (holder == null)
			{
				holder = new GameObject (string.Format ("Pool({0})", pool.prefab.name)).transform;
				holder.parent = root;
				holders.Add (pool, holder);
			}

			if (transform.parent == null)
				transform.parent = holder;
		}

		void Return ()
		{
			Transform holder;
			holders.TryGetValue (pool, out holder);

			if (holder != null)
				transform.parent = holder;
		}
		#endif
	}
}
