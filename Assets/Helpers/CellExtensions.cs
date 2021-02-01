using Assets.Map;
using Assets.Map.Pathing;
using System;
using System.Collections.Generic;
using System.Linq;

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

        internal static IEnumerable<Cell> GetCorners(IEnumerable<Cell> rectangle)
        {
            var minx = rectangle.Min(c => c.Coord.X);
            var minz = rectangle.Min(c => c.Coord.Z);
            var maxx = rectangle.Max(c => c.Coord.X);
            var maxz = rectangle.Max(c => c.Coord.Z);

            var topLeft = rectangle.First(c => c.Coord.X == maxx && c.Coord.Z == minz);
            var topRight = rectangle.First(c => c.Coord.X == maxx && c.Coord.Z == maxz);
            var bottomLeft = rectangle.First(c => c.Coord.X == minx && c.Coord.Z == minz);
            var bottomRight = rectangle.First(c => c.Coord.X == minx && c.Coord.Z == maxz);

            return new Cell[] { topLeft, topRight, bottomLeft, bottomRight };
        }

        internal static IEnumerable<Cell> GetCardinalsOutsideRectangle(IEnumerable<Cell> rectangle)
        {
            var corners = GetCorners(rectangle);
            var anchors = corners.SelectMany(c => c.GetCardinalNeighbours());
            anchors = anchors.Except(rectangle).Distinct();

            return anchors;
        }
    }
}