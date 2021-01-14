using Assets.Map.Pathing;
using System.Collections.Generic;

namespace Assets.Map
{
    public interface IMapManager
    {
        int Height { get; set; }
        int Width { get; set; }

        void Create(Cell[,] cellsToRender);
        void DestroyChunks();
        Cell GetCellAtCoord(int x, int z);
        Cell GetCellAtCoord(ICoord coord);
        Cell GetCenter();
        List<Cell> GetCircle(Cell center, int radius);
        Pathfinder GetPathfinder();
        Cell GetRandomCell();
        Cell GetRandomCell(MapDelegates.CheckCell predicate);
        ChunkRenderer GetRendererForCell(Cell cell);
    }
}