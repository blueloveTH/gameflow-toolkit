using UnityEngine;
using UnityEngine.UI;

namespace Proto.Example
{
	public class SignalExample : MonoBehaviour
	{
		// a signal example class
		// that listens to bullet hit/miss events
		// keeps a score by itself (separately from other logic)
		// and displays it on an UI.Text
		//------------------------------------------------------

		Text text;

		//-----------------------------------------------------

		void Awake ()
		{
			text = GetComponentInChildren<Text> ();
		}

		//-----------------------------------------------------

		void OnEnable ()
		{
			GameSignals.BulletHit.AddListener (RegisterHit);
			GameSignals.BulletMiss.AddListener (RegisterMiss);
		}

		void OnDisable ()
		{
			GameSignals.BulletHit.RemoveListener (RegisterHit);
			GameSignals.BulletMiss.RemoveListener (RegisterMiss);
		}

		//-----------------------------------------------------

		int hits = 0;
		int misses = 0;

		//-----------------------------------------------------

		void UpdateText ()
		{
			text.text = string.Format ("{0} Hits\n{1} Misses", hits, misses);
		}

		//-----------------------------------------------------

		void RegisterHit ()
		{
			hits++;
			UpdateText ();
		}

		void RegisterMiss ()
		{
			misses++;
			UpdateText ();
		}
	}
}