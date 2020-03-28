using System;
using UnityEngine;

namespace STL.Geometry
{
    public struct Line3D
    {
        public Vector3 origin { get; private set; }
        public Vector3 direction { get; private set; }

        public Line3D(Vector3 origin, Vector3 direction)
        {
            this.origin = origin;
            this.direction = direction;
        }

        public Line3D(Ray ray)
        {
            origin = ray.origin;
            direction = ray.direction;
        }

        public void Set2Points(Vector3 p0, Vector3 p1)
        {
            this.direction = p1 - p0;
            this.origin = p0;
        }

        public Vector3 EvalX(float x)
        {
            Vector3 result;
            result.x = x;
            float q = (x - origin.x) / direction.x;
            result.y = q * direction.y + origin.y;
            result.z = q * direction.z + origin.z;
            return result;
        }
        public Vector3 EvalY(float y)
        {
            Vector3 result;
            result.y = y;
            float q = (y - origin.y) / direction.y;
            result.x = q * direction.x + origin.x;
            result.z = q * direction.z + origin.z;
            return result;
        }
        public Vector3 EvalZ(float z)
        {
            Vector3 result;
            result.z = z;
            float q = (z - origin.z) / direction.z;
            result.x = q * direction.x + origin.x;
            result.y = q * direction.y + origin.y;
            return result;
        }

        public bool Intersect(Plane3D plane3D, out Vector3 ip)
        {
            ip = new Vector3();
            float m = Vector3.Dot(direction, plane3D.normal);
            if (m == 0) return false;
            float d = Vector3.Dot(plane3D.origin - origin, plane3D.normal) / m;
            ip = d * direction.normalized + origin;
            return true;
        }
    };

}
