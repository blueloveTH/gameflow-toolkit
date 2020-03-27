//-----------------------------------------------------
// Proto* framework (c) 2016 Raziel Anarki
//-----------------------------------------------------

using System;
using System.Linq;

namespace Proto
{
	// compile-time type-safe event signal system
	// based on the Signal implementation from strangeioc
	//-----------------------------------------------------
	// meh, too bad delegate types cannot be used as generic constraints in C#
	//-----------------------------------------------------

	//-----------------------------------------------------
	// Signal with Action callback

	public class Signal
	{
		event Action Listener = delegate {};
		event Action OnceListener = delegate {};

		public void AddListener (Action callback)
		{
			Listener = AddUnique (Listener, callback);
		}

		public void AddOnce (Action callback)
		{
			OnceListener = AddUnique (OnceListener, callback);
		}

		public void RemoveListener (Action callback)
		{
			Listener -= callback; 
			OnceListener -= callback;
		}

		Action AddUnique (Action listener, Action callback)
		{
			if (!listener.GetInvocationList ().Contains (callback))
				listener += callback;

			return listener;
		}

		public void Dispatch ()
		{
			Listener ();

			OnceListener ();
			OnceListener = delegate
			{
			};
		}
	}

	//-----------------------------------------------------
	// Signal<T> with Action<T> callback

	public class Signal<T>
	{
		event Action<T> Listener = delegate {};
		event Action<T> OnceListener = delegate {};

		public void AddListener (Action<T> callback)
		{
			Listener = AddUnique (Listener, callback);
		}

		public void AddOnce (Action<T> callback)
		{
			OnceListener = AddUnique (OnceListener, callback);
		}

		public void RemoveListener (Action<T> callback)
		{
			Listener -= callback;
			OnceListener -= callback;
		}

		Action<T> AddUnique (Action<T> listener, Action<T> callback)
		{
			if (!listener.GetInvocationList ().Contains (callback))
				listener += callback;

			return listener;
		}

		public void Dispatch (T t)
		{
			Listener (t);

			OnceListener (t);
			OnceListener = delegate
			{
			};
		}
	}

	//-----------------------------------------------------
	// Signal<T,U> with Action<T1,T2> callback

	public class Signal<T,U>
	{
		event Action<T,U> Listener = delegate {};
		event Action<T,U> OnceListener = delegate {};

		public void AddListener (Action<T,U> callback)
		{
			Listener = AddUnique (Listener, callback);
		}

		public void AddOnce (Action<T,U> callback)
		{
			OnceListener = AddUnique (OnceListener, callback);
		}

		public void RemoveListener (Action<T,U> callback)
		{
			Listener -= callback;
			OnceListener -= callback;
		}

		Action<T,U> AddUnique (Action<T,U> listener, Action<T,U> callback)
		{
			if (!listener.GetInvocationList ().Contains (callback))
				listener += callback;

			return listener;
		}

		public void Dispatch (T t, U u)
		{
			Listener (t, u);

			OnceListener (t, u);
			OnceListener = delegate
			{
			};
		}
	}

	//-----------------------------------------------------
	// Signal<T,U,V> with Action<T1,T2,T3> callback

	public class Signal<T,U,V>
	{
		event Action<T,U,V> Listener = delegate {};
		event Action<T,U,V> OnceListener = delegate {};

		public void AddListener (Action<T,U,V> callback)
		{
			Listener = AddUnique (Listener, callback);
		}

		public void AddOnce (Action<T,U,V> callback)
		{
			OnceListener = AddUnique (OnceListener, callback);
		}

		public void RemoveListener (Action<T,U,V> callback)
		{
			Listener -= callback;
			OnceListener -= callback;
		}

		Action<T,U,V> AddUnique (Action<T,U,V> listener, Action<T,U,V> callback)
		{
			if (!listener.GetInvocationList ().Contains (callback))
				listener += callback;

			return listener;
		}

		public void Dispatch (T t, U u, V v)
		{
			Listener (t, u, v);

			OnceListener (t, u, v);
			OnceListener = delegate
			{
			};
		}
	}

	//-----------------------------------------------------
	// Signal<T,U,V,W> with Action<T1,T2,T3,T4> callback

	public class Signal<T,U,V,W>
	{
		event Action<T,U,V,W> Listener = delegate {};
		event Action<T,U,V,W> OnceListener = delegate {};

		public void AddListener (Action<T,U,V,W> callback)
		{
			Listener = AddUnique (Listener, callback);
		}

		public void AddOnce (Action<T,U,V,W> callback)
		{
			OnceListener = AddUnique (OnceListener, callback);
		}

		public void RemoveListener (Action<T,U,V,W> callback)
		{
			Listener -= callback;
			OnceListener -= callback;
		}

		Action<T,U,V,W> AddUnique (Action<T,U,V,W> listener, Action<T,U,V,W> callback)
		{
			if (!listener.GetInvocationList ().Contains (callback))
				listener += callback;

			return listener;
		}

		public void Dispatch (T t, U u, V v, W w)
		{
			Listener (t, u, v, w);

			OnceListener (t, u, v, w);
			OnceListener = delegate
			{
			};
		}
	}
}