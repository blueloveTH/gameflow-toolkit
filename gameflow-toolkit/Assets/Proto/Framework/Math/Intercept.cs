//-----------------------------------------------------
// Proto* framework (c) 2016 Raziel Anarki
//-----------------------------------------------------

using UnityEngine;

namespace Proto
{
	public static class Intercept
	{
		// intercept point calculation
		//------------------------------------------------------

		public static Vector3 Point (Vector3 shooterPosition, Vector3 shooterVelocity, float shotSpeed, Vector3 targetPosition, Vector3 targetVelocity)
		{
			Vector3 relativePosition = targetPosition - shooterPosition;
			Vector3 relativeVelocity = targetVelocity - shooterVelocity;

			return targetPosition + (relativeVelocity) * Time (shotSpeed, relativePosition, relativeVelocity);
		}

		public static float Time (float shotSpeed, Vector3 relativePosition, Vector3 relativeVelocity)
		{
			if (relativeVelocity.sqrMagnitude < 1e-3f)
				return 0f;
            
			float a = relativeVelocity.sqrMagnitude - shotSpeed * shotSpeed;
			float b = 2f * Vector3.Dot (relativeVelocity, relativePosition);
			float c = relativePosition.sqrMagnitude;
            
			if (Mathf.Abs (a) < 1e-3f)
				return Mathf.Max (-c / b, 0f);
            
			float det = b * b - 4f * a * c;
            
			if (det > 0f)
			{
				float t1 = (-b + Mathf.Sqrt (det)) / (2f * a);
				float t2 = (-b - Mathf.Sqrt (det)) / (2f * a);
                
				return (t1 > 0f
                    ? (t2 > 0f ? Mathf.Min (t1, t2) : t1)
                    : Mathf.Max (t2, 0f));
			}
            
			return (det < 0f ? 0f : Mathf.Max (-b / (2f * a), 0f));
		}
	}
}

