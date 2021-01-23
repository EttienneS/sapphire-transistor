using Assets.Resources;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Map
{
    public interface ITerrain
    {
        Color Color { get; }

        TerrainType Type { get; }

        Dictionary<ResourceType, int> ResourceValue { get; }

        int TravelCost { get; }
    }

    public enum TerrainType
    {
        Water, Sand, Grass, Forrest, Snow, Stone
    }
}