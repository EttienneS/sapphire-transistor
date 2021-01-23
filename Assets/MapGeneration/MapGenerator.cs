﻿using Assets.Factions;
using Assets.Helpers;
using Assets.Map;
using Assets.Structures;
using Assets.Structures.Behaviors;
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
                    var cellHeight = height[x, z];
                    var terrrain = _terrainDefinition.GetTerrainTypeForHeight(cellHeight);
                    map[x, z] = new Cell(x, z, terrrain.Type == TerrainType.Water ? -0.25f : 0, terrrain);
                }
            }
            _mapManager.Create(map);
        }     

        internal void PopulateMap(IMapManager mapManager, IStructureFactory structureFactory, IFactionManager factionManager)
        {
            var natureFaction = factionManager.GetNatureFaction();
            var natureFacadeManager = new NatureFacadeManager(structureFactory);

            for (int x = 0; x < mapManager.Width; x++)
            {
                for (int z = 0; z < mapManager.Height; z++)
                {
                    var cell = mapManager.GetCellAtCoord(new Coord(x, 0, z));

                    if (cell.Terrain.Type == TerrainType.Forrest)
                    {
                        if (UnityEngine.Random.value > 0.75f)
                        {
                            natureFaction.StructureManager.AddStructure(natureFacadeManager.GetTree(), cell.Coord);
                        }
                    }
                    if (cell.Terrain.Type == TerrainType.Stone)
                    {
                        if (UnityEngine.Random.value > 0.75f)
                        {
                            natureFaction.StructureManager.AddStructure(natureFacadeManager.GetRock(), cell.Coord);
                        }
                    }
                    if (cell.Terrain.Type == TerrainType.Grass)
                    {
                        if (UnityEngine.Random.value > 0.99f)
                        {
                            natureFaction.StructureManager.AddStructure(natureFacadeManager.GetTree(), cell.Coord);
                        }
                    }
                }
            }
        }
    }
}