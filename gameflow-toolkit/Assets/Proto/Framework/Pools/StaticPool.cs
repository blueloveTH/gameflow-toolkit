//-----------------------------------------------------
// Proto* framework (c) 2016 Raziel Anarki
//-----------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Proto
{
	public static class StaticPool
	{
		// global pool for reusable objects
		//-----------------------------------------------------
		// use (T instance = StaticPool.Request<T> () ?? new T ()); to request an instance
		// reinitialize instances using a late constructor ex. (T)instance.Construct(...); before using
		// call StaticPool.Return (this); from ex. (IDisposable)instance.Dispose (); to return
		//-----------------------------------------------------

		class Pool
		{
			public int count { get; private set; }

			//-----------------------------------------------------

			List<object> pool;

			//-----------------------------------------------------

			public Pool ()
			{
				this.count = 0;
				this.pool = new List<object> (); 
			}

			//-----------------------------------------------------

			public object Request ()
			{
				if (pool.Count > 0)
				{
					int last = pool.Count - 1;

					object item = pool[last];
					pool.RemoveAt (last);

					return item;  
				}

				count++;
				return null;
			}

			//-----------------------------------------------------

			public void Return (object instance)
			{
				pool.Add (instance);
			}
		}

		//-----------------------------------------------------

		static Dictionary<Type,Pool> pooled = new Dictionary<Type,Pool> ();

		//-----------------------------------------------------

		static public T Request<T> ()
		{
			Type type = typeof (T);

			Pool pool;
			pooled.TryGetValue (type, out pool);

			if (pool == null)
			{
				pool = new Pool ();
				pooled.Add (type, pool);
			}

			return (T) pool.Request ();
		}

		//-----------------------------------------------------

		static public void Return (object instance)
		{
			Type type = instance.GetType ();

			Pool pool;
			pooled.TryGetValue (type, out pool);

			if (pool != null)
				pool.Return (instance);
		}
	}
}
