using System.Collections.Generic;
using Proto;
using Proto.AI;
using UnityEngine;

namespace Proto.Example
{
	public class Bullet : Thinker
	{
		// a bullet with a "detachable" trail that doesn't disappear on hit
		//------------------------------------------------------

		public float lifeTime = 2f;
		public float speed = 5f;
		public Hit hitPrefab;
		public Trail trailPrefab;

		//------------------------------------------------------

		Rigidbody body;
		Trail trail;

		void Awake ()
		{
			body = GetComponent<Rigidbody> ();
		}

		//------------------------------------------------------

		protected override void OnEnable ()
		{				
			body.velocity = transform.forward * speed;
			body.angularVelocity = Vector3.zero;

			Hit hit = PoolManager.Spawn<Hit> (hitPrefab, transform.position, transform.rotation);
			hit.gameObject.SetActive (true);

			base.OnEnable ();
		}

		//------------------------------------------------------

		protected override IEnumerable<Routine> Think ()
		{
			trail = PoolManager.Spawn<Trail> (trailPrefab, transform.position, transform.rotation);
			trail.transform.SetParent (transform);
			trail.gameObject.SetActive (true);

			yield return Subroutine.Run (Wait (lifeTime));

			trail.transform.parent = null;

			gameObject.SetActive (false);

			GameSignals.BulletMiss.Dispatch ();
		}

		//------------------------------------------------------

		void OnCollisionEnter (Collision collision)
		{
			Target target = collision.collider.GetComponentInParent<Target> ();

			if (target != null)
				GameSignals.BulletHit.Dispatch ();
			else
				GameSignals.BulletMiss.Dispatch ();

			Hit hit = PoolManager.Spawn<Hit> (hitPrefab, transform.position, transform.rotation);
			hit.gameObject.SetActive (true);

			if (trail != null)
				trail.transform.parent = null;

			gameObject.SetActive (false);
		}
	}
}