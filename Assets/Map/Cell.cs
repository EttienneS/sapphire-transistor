using Assets.Map.Pathing;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Map
{
    public class Cell : PathableCell
    {
        private List<Cell> _nonNullNeighbours;

        private Terrain _terrain;

        public Cell(int x, int z, float height, Terrain terain)
        {
            X = x;
            Z = z;
            Y = height;
            _terrain = terain;
        }

        public new List<Cell> NonNullNeighbors
        {
            get
            {
                if (_nonNullNeighbours == null)
                {
                    _nonNullNeighbours = base.NonNullNeighbors.ConvertAll(c => c as Cell).ToList();
                }
                return _nonNullNeighbours;
            }
        }

        public override float TravelCost
        {
            get
            {
                return Y;
            }
        }

        public Terrain GetTerrain()
        {
            return _terrain;
        }
    }
}