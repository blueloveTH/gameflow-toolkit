//-----------------------------------------------------
// Proto* framework (c) 2016 Raziel Anarki
//-----------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

namespace Proto
{
	// TODO: pool size management

	public static class PoolManager
	{
		// pool manager for multiple prefabs
		//-----------------------------------------------------

		public class Pool
		{
			public GameObject prefab { get; private set; }

			public int count { get; private set; }

			//-----------------------------------------------------

			List<GameObject> pool;

			//-----------------------------------------------------

			public Pool (GameObject prefab)
			{
				this.prefab = prefab;
				this.count = 0;
				this.pool = new List<GameObject> ();
			}

			//-----------------------------------------------------

			public GameObject Request ()
			{		
				GameObject instance;
					
				if (pool.Count > 0)
				{
					int last = pool.Count - 1;

					instance = pool[last];
					pool.RemoveAt (last);
				}
				else
				{
					// haaack
					bool active = prefab.activeSelf;
					prefab.SetActive (false);
					instance = GameObject.Instantiate<GameObject> (prefab);
					prefab.SetActive (active);

					instance.name = prefab.name + "(Pooled)";
					//instance.SetActive (false);  

					instance.AddComponent<Pooled> ().pool = this;

					count++;
				}

				return instance;
			}

			//-----------------------------------------------------

			public void Return (GameObject instance)
			{
				if (instance.activeSelf)
					instance.SetActive (false);

				pool.Add (instance);
			}

		}

		//-----------------------------------------------------

		static Dictionary<GameObject,Pool> pooled = new Dictionary<GameObject,Pool> ();

		//-----------------------------------------------------

		public static GameObject Spawn (GameObject prefab)
		{
			Pool pool;

			if (!pooled.TryGetValue (prefab, out pool))
			{
				pool = new Pool (prefab);
				pooled.Add (prefab, pool);
			}

			return pool.Request ();
		}

		public static T Spawn<T> (T prefab) where T : Component
		{
			return Spawn (prefab.gameObject).GetComponent<T> ();
		}

		//-----------------------------------------------------

		public static GameObject Spawn (GameObject prefab, Vector3 position, Quaternion rotation)
		{
			GameObject instance = Spawn (prefab);

			instance.transform.position = position;
			instance.transform.rotation = rotation;

			return instance;
		}

		public static T Spawn<T> (T prefab, Vector3 position, Quaternion rotation) where T : Component
		{
			return Spawn (prefab.gameObject, position, rotation).GetComponent<T> ();
		}
	}

	//-----------------------------------------------------
	/** /
	public class SomeObject : MonoBehaviour
	{
		GameObject prefab;

		void SomeFunction ()
		{
			// intanstiate disabled gameobject from pool
			GameObject go = PoolManager.Spawn (prefab);

			// reset stuff
			go.transform.position = transform.position; // etc

			// activate gameobject
			go.SetActive (true);

			// use
			// ...

			// when done, return to pool via Pooled.OnDisabled ()
			go.SetActive (false);
		}
	}
	/**/
}
