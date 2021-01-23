using Assets.Resources;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Map
{
    public class Terrain : ITerrain
    {
        public Terrain(TerrainType type, Color color, int travelCost, params (ResourceType resource, int amount)[] resourceValue)
        {
            Type = type;
            Color = color;
            TravelCost = travelCost;

            ResourceValue = resourceValue.ToDictionary(r => r.resource, r => r.amount);
        }

        public TerrainType Type { get; }

        public Dictionary<ResourceType, int> ResourceValue { get; }

        public Color Color { get; }

        public int TravelCost { get; }
    }
}