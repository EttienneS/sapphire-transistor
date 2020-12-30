using Assets.Helpers;
using Assets.Map;
using System;

namespace Assets.MapGeneration
{
    public class MapGenerator
    {
        private readonly IMapManager _mapManager;
        private readonly ITerrainDefinition _terrainDefinition;
        private readonly int _chunksToRender = 30;
        private Cell[,] map;

        public MapGenerator(IMapManager mapManager, int chunksToRender, ITerrainDefinition terrainDefinition)
        {
            _mapManager = mapManager;
            _chunksToRender = chunksToRender;
            _terrainDefinition = terrainDefinition;
        }

        public void GenerateMap()
        {
            var mapSize = Constants.ChunkSize * _chunksToRender;
            map = new Cell[mapSize, mapSize];

            var noise = new FastNoiseLite(Guid.NewGuid().GetHashCode());
            noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);
            noise.SetFrequency(0.01f);
            noise.SetFractalType(FastNoiseLite.FractalType.FBm);

            var height = noise.GetNoiseMap(mapSize);

            for (var x = 0; x < mapSize; x++)
            {
                for (var z = 0; z < mapSize; z++)
                {
                    var cellHeight = GetAdjustedCellHeight(height[x, z]);
                    var terrrain = _terrainDefinition.GetTerrainTypeForHeight(cellHeight);
                    map[x, z] = new Cell(x, z, 0, terrrain);
                }
            }

            _mapManager.Create(map);
        }

        private float GetAdjustedCellHeight(float height)
        {
            var cellHeight = height * 10f;

            if (cellHeight <= 0)
            {
                cellHeight = 0;
            }

            return cellHeight;
        }
    }
}