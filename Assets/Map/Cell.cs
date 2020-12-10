using Assets.Map.Pathing;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Map
{
    public class Cell : PathableCell, ICell
    {
        private readonly ITerrain _terrain;
        private List<ICell> _nonNullNeighbours;

        public List<ICell> GetNonNullNeighbors()
        {
            return _nonNullNeighbours ?? (_nonNullNeighbours = Neighbors.Where(n => n != null).ToList().ConvertAll(c => c as ICell).ToList());
        }

        public Cell(int x, int z, float height, ITerrain terain)
        {
            X = x;
            Z = z;
            Y = height;
            _terrain = terain;
        }

        public override float TravelCost
        {
            get
            {
                return Y;
            }
        }

        public ITerrain GetTerrain()
        {
            return _terrain;
        }
    }
}