using Assets.Resources;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Map
{
    public class Terrain : ITerrain
    {
        public Terrain(string name, Color color, int travelCost, params (ResourceType resource, int amount)[] resourceValue)
        {
            Name = name;
            Color = color;
            TravelCost = travelCost;

            ResourceValue = resourceValue.ToDictionary(r => r.resource, r => r.amount);
        }

        public string Name { get; }

        public Dictionary<ResourceType, int> ResourceValue { get; }

        public Color Color { get; }

        public int TravelCost { get; }
    }
}