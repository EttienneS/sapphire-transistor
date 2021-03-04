using Assets.Factions;
using Assets.Helpers;
using Assets.Map;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Terrain = Assets.Map.Terrain;

namespace Assets.MapGeneration
{
    public class DefaultTerrainDefinition : ITerrainDefinition
    {
        private Dictionary<TerrainType, Terrain> _terrainLookup;
        private Dictionary<TerrainType, (float min, float max)> _heightLookup;

        public DefaultTerrainDefinition()
        {
            _heightLookup = new Dictionary<TerrainType, (float min, float max)>
            {
                { TerrainType.Snow, (0.9f, float.MaxValue) },
                { TerrainType.Stone, (0.7f, 0.9f) },
                { TerrainType.Forrest, (0.5f, 0.7f) },
                { TerrainType.Grass, (0.1f, 0.5f) },
                { TerrainType.Sand, (0, 0.1f) },
                { TerrainType.Water, (float.MinValue, 0f) },
            };
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

        public float GetHeightForType(TerrainType type)
        {
            var (min, max) = _heightLookup[type];
            return RandomExtensions.Range(min, max);
        }

        public ITerrain GetTerrainTypeByType(TerrainType type)
        {
            return _terrainLookup[type];
        }

        public ITerrain GetTerrainTypeForHeight(float cellHeight)
        {
            foreach (var kvp in _heightLookup)
            {
                if (cellHeight >= kvp.Value.min && cellHeight < kvp.Value.max)
                {
                    return _terrainLookup[kvp.Key];
                }
            }

            throw new KeyNotFoundException($"Height lookup not found: {cellHeight}");
        }
    }
}