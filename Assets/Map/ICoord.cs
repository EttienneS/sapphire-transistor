using System;

namespace Assets.Map
{
    public interface ICoord
    {
        float Y { get; }
        int X { get; }
        int Z { get; }
    }
}