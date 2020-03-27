using System;
using System.Collections.Generic;
using Proto;
using Proto.AI;
using UnityEngine;

namespace Proto.Example
{
    public class Turret : Thinker
    {
        // the example turret, with less comments (see manual)
        // and target interception added in one line
        //------------------------------------------------------

        public Bullet bulletPrefab;
        public GameObject trackingLight;
        public Transform barrel;

        public float targetRadius = 10f;

        public float trackSpeed = 2f;
        public float shotDelay = 1f;

        //-----------------------------------------------------

        protected override void OnEnable()
        {
            trackingLight.SetActive(false);
            base.OnEnable();
        }

        //------------------------------------------------------

        protected override void FixedUpdate()
        {
            // this makes the profiler show the subroutines of this class
            // under it's own row instead under Thinker.FixedUpdate ();
            base.FixedUpdate();
        }

        //-----------------------------------------------------

        protected override IEnumerable<Routine> Think()
        {
            // save turret start angle
            Vector3 startAngle = transform.forward;

            while (true)
            {
                // look for a target
                Transform target = null;

                yield return Until.Run(
                    FindTargetInRadius(targetRadius, (found) => target = found),
                    Idle(startAngle)
                );

                if (target != null)
                {
                    // we have a target
                    // track and shoot simoultaneously
                    yield return While.Run(
                        () => Vector3.Distance(target.position, transform.position) < targetRadius,
                        Parallel.Run(
                            TrackTarget(target),
                            FireProjectiles()
                        )
                    );

                    // target left the area, look for another
                    target = null;
                }
            }
        }

        //-----------------------------------------------------

        IEnumerable<Routine> FindTargetInRadius(float radius, Action<Transform> foundTarget)
        {
            Collider target = null;

            while (target == null)
            {
                List<Collider> colliders = new List<Collider>(Physics.OverlapSphere(transform.position, targetRadius));

                target = colliders.Find((collider) => collider.GetComponentInParent<Target>() != null);

                yield return null;
            }

            foundTarget(target.GetComponentInParent<Target>().transform);
        }

        //-----------------------------------------------------

        IEnumerable<Routine> Idle(Vector3 startAngle)
        {
            while (true)
            {
                transform.forward = Vector3.RotateTowards(transform.forward, startAngle, Time.deltaTime * trackSpeed, 0f);
                yield return null;
            }
        }

        //-----------------------------------------------------

        IEnumerable<Routine> TrackTarget(Transform target)
        {
            try
            {
                trackingLight.SetActive(true);

                while (true)
                {
                    Vector3 position = Intercept.Point(barrel.position, Vector3.zero, bulletPrefab.speed, target.position, target.GetComponent<UnityEngine.AI.NavMeshAgent>().velocity);
                    Vector3 direction = (position - transform.position).normalized;
                    transform.forward = Vector3.RotateTowards(transform.forward, direction, Time.deltaTime * trackSpeed, 0f);
                    yield return null;
                }
            }
            finally
            {
                // disable tracking light when tacking is interrupted
                trackingLight.SetActive(false);
            }
        }

        //-----------------------------------------------------

        IEnumerable<Routine> FireProjectiles()
        {
            while (true)
            {
                // spawn a projectile
                Bullet bullet = PoolManager.Spawn<Bullet>(bulletPrefab, barrel.position, barrel.rotation);
                bullet.gameObject.SetActive(true);

                yield return Subroutine.Run(Wait(shotDelay));
            }
        }
    }
}
