using System;
using System.Linq;

namespace Assets.Map.Pathing
{
    public abstract class PathableCell : IPathFindableCell, IEquatable<PathableCell>
    {
        public PathableCell[] Neighbors = new PathableCell[8];

        private int _hash;

        public IPathFindableCell NextWithSamePriority { get; set; }

        public IPathFindableCell PathFrom { get; set; }
        public float SearchDistance { get; set; }
        public int SearchHeuristic { get; set; }

        public int SearchPhase { get; set; }

        public int SearchPriority => (int)SearchDistance + SearchHeuristic;

        public abstract float TravelCost { get; }

        public ICoord Coord { get; set; }

        public static bool operator !=(PathableCell obj1, PathableCell obj2)
        {
            if (ReferenceEquals(obj1, null))
            {
                return !ReferenceEquals(obj2, null);
            }

            return !obj1.Equals(obj2);
        }

        public static bool operator ==(PathableCell obj1, PathableCell obj2)
        {
            if (ReferenceEquals(obj1, null))
            {
                return ReferenceEquals(obj2, null);
            }

            return obj1.Equals(obj2);
        }

        public int DistanceTo(IPathFindableCell other)
        {
            // to handle cases where a diagonal does not count as adjecent
            if (Neighbors.Contains(other))
            {
                return 1;
            }

            return (Coord.X < other.Coord.X ? other.Coord.X - Coord.X : Coord.X - other.Coord.X) +
                   (Coord.Z < other.Coord.Z ? other.Coord.Z - Coord.Z : Coord.Z - other.Coord.Z);
        }

        public bool Equals(PathableCell other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return Coord.X == other.Coord.X && Coord.Z == other.Coord.Z;
        }

        public override bool Equals(object obj)
        {
            var other = obj as PathableCell;
            if (other == null)
            {
                return false;
            }

            return this == other;
        }

        public override int GetHashCode()
        {
            // pre-calculated hash to improve search speed;
            if (_hash == 0)
            {
                // pre-calculate the hash as it will not change over time
                _hash = $"{Coord.X}:{Coord.Z}".GetHashCode();
            }

            return _hash;
        }

        public IPathFindableCell GetNeighbor(Direction direction)
        {
            return Neighbors[(int)direction];
        }

        public void SetNeighbor(Direction direction, PathableCell cell)
        {
            Neighbors[(int)direction] = cell;
            cell.Neighbors[(int)direction.Opposite()] = this;
        }
    }
}