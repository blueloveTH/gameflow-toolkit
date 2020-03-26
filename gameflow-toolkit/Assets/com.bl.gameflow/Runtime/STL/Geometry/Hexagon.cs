using System;
using UnityEngine;

namespace STL.Geometry
{
    public enum HexDir
    {
        NULL = -1,
        UP_LEFT = 0,
        UP_RIGHT,
        RIGHT,
        DOWN_RIGHT,
        DOWN_LEFT,
        LEFT,
    };

    public class Hexagon
    {
        //从UpLeft顺时针排列的转换表
        private static readonly Vector3Int[] transTable = new Vector3Int[6]
        {
            new  Vector3Int(0, 1, -1),
            new  Vector3Int(1, 0, -1),
            new  Vector3Int(1, -1, 0),
            new  Vector3Int(0, -1, 1),
            new  Vector3Int(-1, 0, 1),
            new  Vector3Int(-1, 1, 0)
        };

        public Vector3Int[] vertexs = new Vector3Int[6];
        public Hexagon(Vector3Int center, int radius)
        {
            for (int i = 0; i < 6; i++)
            {
                vertexs[i] = center + transTable[i] * radius;
            }
        }

        public static Vector3Int TransTable(HexDir t) { return transTable[(int)t]; }
        public static Vector3Int TransTable(int i) { return transTable[i]; }
        public static int TransTable(Vector3Int dir)
        {
            for (int i = 0; i < 6; i++)
            {
                if (transTable[i] == dir) return i;
            }
            return -1;
        }

        public static HexDir Reverse(HexDir dir) { return (HexDir)(((int)dir + 3) % 6); }
        public static int Reverse(int dir) { return (dir + 3) % 6; }
        public static HexDir Next(HexDir dir)
        {
            dir = (HexDir)((int)dir + 1);
            if ((int)dir > 5) dir = (HexDir)0;
            return dir;
        }
        public static HexDir Previous(HexDir dir)
        {
            dir = (HexDir)((int)dir - 1);
            if ((int)dir < 0) dir = (HexDir)5;
            return dir;
        }
    };


}
