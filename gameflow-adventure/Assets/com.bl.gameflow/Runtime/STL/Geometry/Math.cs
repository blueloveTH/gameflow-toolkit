using System;
using UnityEngine;

namespace STL.Geometry
{
    public struct Math
    {
        public static Vector2 RotateAroundPoint(Vector2 v, Vector2 center, float angle, bool clockwise)
        {
            Vector2 v_moveToZero = v - center;
            return RotateAroundZero(v_moveToZero, angle, clockwise) + center;
        }
        public static Vector2 RotateAroundZero(Vector2 v, float angle, bool clockwise)
        {
            float angleInRadians = angle * Mathf.Deg2Rad;
            float sinA = Mathf.Sin(angleInRadians);
            float cosA = Mathf.Cos(angleInRadians);
            if (clockwise)
                return new Vector2(v.x * cosA + v.y * sinA, -v.x * sinA + v.y * cosA);
            else
                return new Vector2(v.x * cosA - v.y * sinA, v.x * sinA + v.y * cosA);
        }

        public static float PlaneDistance(Vector3 p0, Vector3 p1)
        {
            float dx = p0.x - p1.x;
            float dz = p0.z - p1.z;
            return Mathf.Sqrt(dx * dx + dz * dz);
        }
        public static float SqrPlaneDistance(Vector3 p0, Vector3 p1)
        {
            float dx = p0.x - p1.x;
            float dz = p0.z - p1.z;
            return dx * dx + dz * dz;
        }

        public static Vector3 RotateAroundAxis(Vector3 v, Vector3 axis, float angle)
        {
            v = Quaternion.AngleAxis(angle, axis) * v;
            return v;
        }
        public static Vector2 Mirror(Vector2 v, Vector2 axis)
        {
            return RotateAroundAxis(v, axis, 180);
        }
    };
}