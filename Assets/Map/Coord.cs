using System;

namespace Assets.Map
{
    [Serializable]
    public class Coord : IEquatable<Coord>
    {
        private int _hash = 0;

        public Coord(int x, float y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; }
        public float Y { get; internal set; }
        public int Z { get; }

        public static bool operator !=(Coord obj1, Coord obj2)
        {
            if (obj1 is null)
            {
                return obj2 is object;
            }

            return !obj1.Equals(obj2);
        }

        public static bool operator ==(Coord obj1, Coord obj2)
        {
            if (obj1 is null)
            {
                return obj2 is null;
            }

            return obj1.Equals(obj2);
        }

        public int DistanceTo(Coord other)
        {
            return (X < other.X ? other.X - X : X - other.X) +
                    (Z < other.Z ? other.Z - Z : Z - other.Z);
        }

        public bool Equals(Coord other)
        {
            if (other is null)
            {
                return false;
            }

            return X == other.X && Z == other.Z;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Coord;
            if (other == null)
            {
                return false;
            }

            return this == other;
        }

        internal void SetY(float newY)
        {
            Y = newY;
        }

        public override int GetHashCode()
        {
            // pre-calculated hash to improve search speed;
            if (_hash == 0)
            {
                // pre-calculate the hash as it will not change over time
                _hash = $"{X}:{Z}".GetHashCode();
            }

            return _hash;
        }

        public override string ToString()
        {
            return $"{X}:{Y}:{Z}";
        }
    }
}