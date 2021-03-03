﻿using Assets.Map.Pathing;
using System.Collections.Generic;

namespace Assets.Map
{
    public interface IMapManager
    {
        int Height { get; set; }
        int Width { get; set; }

        void Create(Cell[,] cellsToRender);

        void DestroyChunks();

        bool TryGetCellAtCoord(int x, int z, out Cell cell);

        bool TryGetCellAtCoord(Coord coord, out Cell cell);

        Cell GetCenter();

        List<Cell> GetCircle(Coord coord, int radius);
        Pathfinder GetPathfinder();

        Cell GetRandomCell();

        Cell GetRandomCell(MapDelegates.CheckCell predicate);

        ChunkRenderer GetRendererForCell(Cell cell);

        List<Cell> GetRectangle(Coord coord, int width, int height);
    }
}