using UnityEngine;

namespace Assets.Map
{
    public interface ICoord
    {
        float Y { get; }
        int X { get; }
        int Z { get; }
    }

    public static class CoordExtension
    {
        public static Vector3 ToVector3(this ICoord coord)
        {
            return new Vector3(coord.X, coord.Y, coord.Z);
        }

        public static Vector3 ToAdjustedVector3(this ICoord coord)
        {
            return new Vector3(coord.X, coord.Y, coord.Z) + new Vector3(0.5f, 0, 0.5f);
        }
    }
}