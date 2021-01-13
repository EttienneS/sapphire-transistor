using Assets.Map;
using Assets.Map.Pathing;
using System.Collections.Generic;

namespace Assets.Helpers
{
    public static class CellExtensions
    {
        public static List<Cell> GetCardinalNeighbours(this Cell cell)
        {
            var cardinalNeighbours = new List<Cell>();

            if (cell.TryGetNeighbourInDirection(Direction.N, out Cell north))
            {
                cardinalNeighbours.Add(north);
            }
            if (cell.TryGetNeighbourInDirection(Direction.E, out Cell east))
            {
                cardinalNeighbours.Add(east);
            }
            if (cell.TryGetNeighbourInDirection(Direction.S, out Cell south))
            {
                cardinalNeighbours.Add(south);
            }
            if (cell.TryGetNeighbourInDirection(Direction.W, out Cell west))
            {
                cardinalNeighbours.Add(west);
            }

            return cardinalNeighbours;
        }

        private static bool TryGetNeighbourInDirection(this Cell cell, Direction dir, out Cell neighbour)
        {
            neighbour = cell.GetNeighbor(dir) as Cell;

            return neighbour != null;
        }
    }
}