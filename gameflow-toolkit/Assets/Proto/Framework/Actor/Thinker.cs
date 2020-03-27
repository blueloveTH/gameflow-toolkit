//-----------------------------------------------------
// Proto* framework (c) 2016 Raziel Anarki
//-----------------------------------------------------

using System.Collections.Generic;
using Proto.AI;
using UnityEngine;

namespace Proto
{
    public abstract class Thinker : MonoBehaviour
    {
        // monobehaviour baseclass with a routine "thinker"
        //------------------------------------------------------

        Routine routine;

        //-----------------------------------------------------

        protected virtual void OnEnable()
        {
            routine = Subroutine.Run(Think());
        }

        protected virtual void OnDisable()
        {
            if (routine != null)
            {
                routine.Dispose();
                routine = null;
            }
        }

        //-----------------------------------------------------

        protected virtual void FixedUpdate()
        {
            if (routine != null && routine.running)
                routine.Update();
        }

        //-----------------------------------------------------
        // override this in subclasses

        protected virtual IEnumerable<Routine> Think()
        {
            yield break;
        }

        //-----------------------------------------------------

        protected IEnumerable<Routine> Wait(float seconds)
        {
            float start = Time.time;

            while (Time.time - start < seconds)
                yield return null;
        }
    }
}