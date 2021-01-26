using Assets.Factions;
using Assets.Helpers;
using Assets.Map;
using System.Collections.Generic;
using System.Linq;
using Terrain = Assets.Map.Terrain;

namespace Assets.MapGeneration
{
    public class DefaultTerrainDefinition : ITerrainDefinition
    {
        private Dictionary<TerrainType, Terrain> _terrainLookup;

        public DefaultTerrainDefinition()
        {
            var terrains = new[]
            {
                new Terrain(TerrainType.Snow, "e9ecef".GetColorFromHex(), 4),
                new Terrain(TerrainType.Stone, "6c757d".GetColorFromHex(), 2, (ResourceType.Stone, 1)),
                new Terrain(TerrainType.Forrest, "386641".GetColorFromHex(), 1, (ResourceType.Wood, 2), (ResourceType.Food, 1)),
                new Terrain(TerrainType.Grass, "a7c957".GetColorFromHex(), 1,(ResourceType.Food, 1)),
                new Terrain(TerrainType.Sand, "f2e8cf".GetColorFromHex(), 1),
                new Terrain(TerrainType.Water, "5390d9".GetColorFromHex(), -1, (ResourceType.Food, 1)),
            };

            _terrainLookup = terrains.ToDictionary(t => t.Type, t => t);
        }

        public ITerrain GetTerrainTypeForHeight(float cellHeight)
        {
            TerrainType terrainKey;
            if (cellHeight > 0.9)
            {
                terrainKey = TerrainType.Snow;
            }
            else if (cellHeight > 0.7)
            {
                terrainKey = TerrainType.Stone;
            }
            else if (cellHeight > 0.5)
            {
                terrainKey = TerrainType.Forrest;
            }
            else if (cellHeight > 0.1)
            {
                terrainKey = TerrainType.Grass;
            }
            else if (cellHeight > 0)
            {
                terrainKey = TerrainType.Sand;
            }
            else
            {
                terrainKey = TerrainType.Water;
            }

            return _terrainLookup[terrainKey];
        }
    }
}