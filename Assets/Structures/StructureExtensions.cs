using Assets.Map;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Structures
{
    public static class StructureExtensions
    {
        public static Vector3 CalculatePlacementPosition(ICoord coord, int width, int height)
        {
            var position = coord.ToAdjustedVector3();
            var offsetX = Mathf.Max(0, width - 1) / 2f;
            var offsetZ = Mathf.Max(0, height - 1) / 2f;
            position += new Vector3(offsetX, 0, offsetZ);
            return position;
        }

        public static ICoord[] GetPlacementCoords(this IStructure structure, ICoord coord)
        {
            return GetPlacementCoords(coord, structure.Height, structure.Width);
        }

        public static ICoord[] GetPlacementCoords(ICoord coord, int width, int height)
        {
            var coords = new List<ICoord>();
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    coords.Add(new Coord(coord.X + x, coord.Y, coord.Z + z));
                }
            }

            return coords.ToArray();
        }
    }
}