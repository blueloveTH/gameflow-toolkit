using System;
using UnityEngine;

namespace STL.Geometry
{
    public struct Plane3D
    {
        public Vector3 origin { get; private set; }
        public Vector3 normal { get; private set; }

        public Plane3D(Vector3 point, Vector3 normal)
        {
            this.origin = point;
            this.normal = normal;
        }

        public void SetPointAndNormal(Vector3 point, Vector3 normal)
        {
            this.origin = point;
            this.normal = normal;
        }
    };


}
