using System;

namespace Assets.Map
{
    [Serializable]
    public class Coord : ICoord
    {
        public Coord(int x, float y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public float Y { get; }
        public int X { get; }
        public int Z { get; }
    }
}