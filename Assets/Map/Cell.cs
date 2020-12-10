using Assets.Map.Pathing;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Map
{
    public class Cell : PathableCell, ICell
    {
        private List<ICell> _nonNullNeighbours;

        public Cell(int x, int z, float height, ITerrain terain)
        {
            X = x;
            Z = z;
            Y = height;
            Terrain = terain;
        }

        public List<ICell> NonNullNeighbors
        {
            get
            {
                return _nonNullNeighbours ?? (_nonNullNeighbours = Neighbors.Where(n => n != null).ToList().ConvertAll(c => c as ICell).ToList());
            }
        }

        public ITerrain Terrain { get; }

        public override float TravelCost
        {
            get
            {
                return Y;
            }
        }
    }
}