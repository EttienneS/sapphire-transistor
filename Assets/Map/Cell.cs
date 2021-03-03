using Assets.Map.Pathing;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Map
{
    public class Cell
    {
        public Cell[] Neighbors = new Cell[8];
        private int _hash;
        private List<Cell> _nonNullNeighbours;

        public Cell(int x, int z, float height, ITerrain terain)
        {
            Coord = new Coord(x, height, z);
            Terrain = terain;
        }

        public Coord Coord { get; set; }

        public Cell NextWithSamePriority { get; set; }

        public List<Cell> NonNullNeighbors
        {
            get
            {
                return _nonNullNeighbours ??= Neighbors.Where(n => n != null).ToList().ConvertAll(c => c as Cell).ToList();
            }
        }

        public Cell PathFrom { get; set; }
        public float SearchDistance { get; set; }
        public int SearchHeuristic { get; set; }
        public int SearchPhase { get; set; }
        public int SearchPriority => (int)SearchDistance + SearchHeuristic;
        public ITerrain Terrain { get; }

        public static bool operator !=(Cell obj1, Cell obj2)
        {
            if (ReferenceEquals(obj1, null))
            {
                return !ReferenceEquals(obj2, null);
            }

            return !obj1.Equals(obj2);
        }

        public static bool operator ==(Cell obj1, Cell obj2)
        {
            if (ReferenceEquals(obj1, null))
            {
                return ReferenceEquals(obj2, null);
            }

            return obj1.Equals(obj2);
        }

        public int DistanceTo(Cell other)
        {
            // to handle cases where a diagonal does not count as adjecent
            if (Neighbors.Contains(other))
            {
                return 1;
            }

            return (Coord.X < other.Coord.X ? other.Coord.X - Coord.X : Coord.X - other.Coord.X) +
                   (Coord.Z < other.Coord.Z ? other.Coord.Z - Coord.Z : Coord.Z - other.Coord.Z);
        }

        public bool Equals(Cell other)
        {
            if (ReferenceEquals(other, null))
            {
                return false;
            }

            return Coord.X == other.Coord.X && Coord.Z == other.Coord.Z;
        }

        public override bool Equals(object obj)
        {
            var other = obj as Cell;
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

        public Cell GetNeighbor(Direction direction)
        {
            return Neighbors[(int)direction];
        }

        public float GetTravelCost()
        {
            return Terrain.TravelCost;
        }

        public void SetNeighbor(Direction direction, Cell cell)
        {
            Neighbors[(int)direction] = cell;
            cell.Neighbors[(int)direction.Opposite()] = this;
        }

        public override string ToString()
        {
            return $"{Coord}: {Terrain.Type}";
        }
    }
}