using System;
using UnityEngine;

namespace STL.Geometry
{
    public struct PolarVector2
    {
        public float length;
        public float angle;

        public PolarVector2(float length, float angle)
        {
            angle = angle % 360;
            if (length < 0)
            {
                length = -length;
                angle += 180;
            }

            this.length = length;
            this.angle = angle;
        }

        public PolarVector2(Vector2 v)
        {
            length = v.magnitude;
            angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        }

        public Vector2 ToVector2()
        {
            float angleInRadians = angle * Mathf.Deg2Rad;
            float x = length * Mathf.Cos(angleInRadians);
            float y = length * Mathf.Sin(angleInRadians);
            return new Vector2(x, y);
        }

        public static PolarVector2 operator +(PolarVector2 lhs, PolarVector2 rhs)
        {
            Vector2 lv = lhs.ToVector2();
            Vector2 rv = rhs.ToVector2();
            return new PolarVector2(lv + rv);
        }

        public static PolarVector2 operator -(PolarVector2 lhs, PolarVector2 rhs)
        {
            Vector2 lv = lhs.ToVector2();
            Vector2 rv = rhs.ToVector2();
            return new PolarVector2(lv - rv);
        }

        public static PolarVector2 operator *(PolarVector2 lhs, float rhs)
        {
            return new PolarVector2(lhs.length * rhs, lhs.angle);
        }

        public static PolarVector2 operator /(PolarVector2 lhs, float rhs)
        {
            return new PolarVector2(lhs.length / rhs, lhs.angle);
        }
    }

}
