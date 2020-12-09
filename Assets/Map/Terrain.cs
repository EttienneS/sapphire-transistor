using Assets.Resources;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Map
{
    public class Terrain : ITerrain
    {
        public Terrain(string name, Color color, params (IResource resource, int amount)[] resourceValue)
        {
            Name = name;
            Color = color;

            ResourceValue = resourceValue.ToDictionary(r => r.resource, r => r.amount);
        }

        public string Name { get; }

        public Dictionary<IResource, int> ResourceValue { get; }

        public Color Color { get; }
    }
}