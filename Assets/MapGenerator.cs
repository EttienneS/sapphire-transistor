﻿using Assets.Helpers;
using Assets.Map;
using Assets.ServiceLocator;
using Assets.StrategyCamera;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Terrain = Assets.Map.Terrain;

public class MapGenerator : LocatableMonoBehavior
{
    public Material ChunkMaterial;

    [Range(5, 30)]
    public int ChunksToRender = 30;

    public AnimationCurve HeightCurve;

    private Dictionary<string, Terrain> _terrainLookup;
    private Cell[,] map;

    public override void Initialize()
    {
        InitializeTerrainLookup();

        GenerateMap();

        MakeCellMagentaOnClick();

        ConfigureMapBounds();
    }

    public void RegenerateMap()
    {
        using (Instrumenter.Start())
        {
            var mapSize = GetMapSize();
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
                    var terrrain = GetTerrainTypeForHeight(cellHeight);
                    map[x, z] = new Cell(x, z, cellHeight, terrrain);
                }
            }

            Locate<ChunkManager>().RenderCells(map);
        }
    }

    private void ConfigureMapBounds()
    {
        var max = GetMapSize();
        var camera = Locate<CameraController>();
        camera.ConfigureBounds(0, max, 0, max + 50);
        camera.MoveToWorldCenter();
    }

    private void GenerateMap()
    {
        GetLocator().Unregister<ChunkManager>();
        GetLocator().Register(ChunkManager.CreateChunkManager(ChunkMaterial));
        RegenerateMap();
    }

    private float GetAdjustedCellHeight(float height)
    {
        var cellHeight = HeightCurve.Evaluate(height) * 20f;

        if (cellHeight <= 0)
        {
            cellHeight = 0;
        }

        return cellHeight;
    }

    private int GetMapSize()
    {
        return Constants.ChunkSize * ChunksToRender;
    }

    private Terrain GetTerrainTypeForHeight(float cellHeight)
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
    private void InitializeTerrainLookup()
    {
        var terrains = new[]
        {
            new Terrain("Snow", Color.white),
            new Terrain("Stone", Color.grey),
            new Terrain("Forrest", ColorExtensions.GetColorFromHex("2d6a4f")),
            new Terrain("Grass", ColorExtensions.GetColorFromHex("52b788")),
            new Terrain("Sand", Color.yellow),
            new Terrain("Water", Color.blue),
        };

        _terrainLookup = terrains.ToDictionary(t => t.GetName(), t => t);
    }

    private void MakeCellMagentaOnClick()
    {
        CellEventManager.OnCellClicked += (cell) =>
        {
            Locate<ChunkManager>().GetRendererForCell(cell).GenerateMesh();
        };
    }
}