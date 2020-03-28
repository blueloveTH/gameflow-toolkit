using System;
using UnityEngine;

namespace STL.Geometry
{
    /// <summary>
    /// Defines a 2D line
    /// </summary>
    public struct Line2D
    {
        private float A, B, C;

        public float k { get { return -A / B; } }
        public bool has_K { get { return B != 0; } }

        /// <summary>
        /// y=kx+b
        /// </summary>
        public Line2D(float k, float b = 0)
        {
            A = k;
            B = -1;
            C = b;
        }

        /// <summary>
        /// Ax+By+C=0
        /// </summary>
        public Line2D(float A, float B, float C)
        {
            if (A == B && A == 0) throw new System.ArgumentException("Invaild: A=B=0");
            this.A = A; this.B = B; this.C = C;
        }

        public Line2D(Vector2 p0, Vector2 p1)
        {
            if (p0 == p1) throw new System.ArgumentException("Invaild: p0=p1");
            A = p1.y - p0.y;
            B = p0.x - p1.x;
            C = p1.x * p0.y - p0.x * p1.y;
        }

        public Line2D(Ray2D ray2D)
        {
            Vector2 p0 = ray2D.origin;
            Vector2 p1 = ray2D.origin + ray2D.direction;

            if (p0 == p1) throw new System.ArgumentException("Invaild: p0=p1");
            A = p1.y - p0.y;
            B = p0.x - p1.x;
            C = p1.x * p0.y - p0.x * p1.y;
        }

        public Line2D Vertical(Vector2 point)
        {
            if (has_K)
            {
                if (k == 0) return new Line2D(point, point + Vector2.up);
                Line2D line = new Line2D(-1 / k);
                line.C = point.y - line.k * point.x;
                return line;
            }
            else return new Line2D(point, point + Vector2.right);
        }

        public Vector2 EvalX(float x)
        {
            if (B == 0) throw new InvalidOperationException("Cannot eval x when B=0");
            return new Vector2(x, (-C - A * x) / B);
        }

        public Vector2 EvalY(float y)
        {
            if (A == 0) throw new InvalidOperationException("Cannot eval y when A=0");
            return new Vector2((-C - B * y) / A, y);
        }

        public bool Intersect(Line2D other, out Vector2 ip)
        {
            ip = Vector2.zero;
            float m = A * other.B - other.A * B;
            if (m == 0) return false;
            else
            {
                ip.x = (other.C * B - C * other.B) / m;
                ip.y = (C * other.A - other.C * A) / m;
            }
            return true;
        }

        public float Distance(Vector2 p)
        {
            Vector2 ip;
            Vertical(p).Intersect(this, out ip);
            return Vector2.Distance(p, ip);
        }
    };

}
