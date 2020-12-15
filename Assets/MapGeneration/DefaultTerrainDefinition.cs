using Assets.Helpers;
using Assets.Resources;
using System.Collections.Generic;
using System.Linq;
using Terrain = Assets.Map.Terrain;

namespace Assets.MapGeneration
{
    public class DefaultTerrainDefinition : ITerrainDefinition
    {
        private Dictionary<string, Terrain> _terrainLookup;

        public DefaultTerrainDefinition()
        {
            var terrains = new[]
            {
                new Terrain("Snow", "e9ecef".GetColorFromHex()),
                new Terrain("Stone", "6c757d".GetColorFromHex(), (ResourceType.Stone, 1)),
                new Terrain("Forrest", "386641".GetColorFromHex(), (ResourceType.Wood, 2), (ResourceType.Food, 1)),
                new Terrain("Grass", "a7c957".GetColorFromHex(), (ResourceType.Food, 1)),
                new Terrain("Sand", "f2e8cf".GetColorFromHex()),
                new Terrain("Water", "5390d9".GetColorFromHex(), (ResourceType.Food, 1)),
            };

            _terrainLookup = terrains.ToDictionary(t => t.Name, t => t);
        }

        public Terrain GetTerrainTypeForHeight(float cellHeight)
        {
            string terrainKey;
            if (cellHeight > 9)
            {
                terrainKey = "Snow";
            }
            else if (cellHeight > 7)
            {
                terrainKey = "Stone";
            }
            else if (cellHeight > 5)
            {
                terrainKey = "Forrest";
            }
            else if (cellHeight > 1)
            {
                terrainKey = "Grass";
            }
            else if (cellHeight > 0)
            {
                terrainKey = "Sand";
            }
            else
            {
                terrainKey = "Water";
            }

            return _terrainLookup[terrainKey];
        }
    }
}