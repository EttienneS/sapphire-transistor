using System;
using UnityEngine;

namespace Assets.Map
{
    public interface ICoord : IEquatable<ICoord>
    {
        float Y { get; }
        int X { get; }
        int Z { get; }
        int DistanceTo(ICoord other);
    }
}