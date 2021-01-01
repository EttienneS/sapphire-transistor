using Assets.Helpers;
using Assets.Map.Pathing;
using Assets.ServiceLocator;
using Assets.StrategyCamera;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Map
{
    public class MapManager : LocatableMonoBehaviorBase, IMapManager
    {
        public Material ChunkMaterial;

        private Dictionary<ICoord, Cell> _cellLookup;
        private Cell[,] _cells;
        private ChunkRendererFactory _chunkRendererFactory;
        private ChunkRenderer[,] _chunkRenderers;
        private List<Cell> _flatCells;

        private Pathfinder _pathfinder;
        public int Height { get; set; }

        public int Width { get; set; }

        public void AddCellIfFound(int x, int z, List<Cell> cells)
        {
            if (x < Width && z < Height && x >= 0 && z >= 0)
            {
                cells.Add(_cells[x, z]);
            }
        }

        public void DestroyChunks()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        public Cell GetCellAtCoord(ICoord coord)
        {
            return _cellLookup[coord];
        }

        public Cell GetCenter()
        {
            return _cells[Width / 2, Height / 2];
        }

        public List<Cell> GetCircle(Cell center, int radius)
        {
            var cells = new List<Cell>();

            var centerX = center.Coord.X;
            var centerY = center.Coord.Z;

            for (var x = centerX - radius; x <= centerX; x++)
            {
                for (var y = centerY - radius; y <= centerY; y++)
                {
                    if ((x - centerX) * (x - centerX) + (y - centerY) * (y - centerY) <= radius * radius)
                    {
                        AddCellIfFound(centerX - (x - centerX), centerY - (y - centerY), cells);
                        AddCellIfFound(centerX + (x - centerX), centerY + (y - centerY), cells);
                        AddCellIfFound(centerX - (x - centerX), centerY + (y - centerY), cells);
                        AddCellIfFound(centerX + (x - centerX), centerY - (y - centerY), cells);
                    }
                }
            }

            return cells;
        }

        public Pathfinder GetPathfinder()
        {
            return _pathfinder;
        }

        public Cell GetRandomCell(MapDelegates.CheckCell predicate)
        {
            return _flatCells.Where(c => predicate.Invoke(c)).GetRandomItem();
        }

        public Cell GetRandomCell()
        {
            return _cells[(int)(Random.value * Width), (int)(Random.value * Height)];
        }

        public ChunkRenderer GetRendererForCell(Cell cell)
        {
            var x = Mathf.FloorToInt(cell.Coord.X / Constants.ChunkSize);
            var z = Mathf.FloorToInt(cell.Coord.Z / Constants.ChunkSize);

            return _chunkRenderers[x, z];
        }

        public override void Initialize()
        {
            _chunkRendererFactory = new ChunkRendererFactory(Locate<ICameraController>().GetCamera());
        }

        public void Create(Cell[,] cellsToRender)
        {
            DestroyChunks();

            _cells = cellsToRender;
            _flatCells = _cells.Flatten().ToList();

            IndexCells(_cells);

            Width = cellsToRender.GetLength(0);
            Height = cellsToRender.GetLength(1);

            var chunkWidth = Mathf.FloorToInt(cellsToRender.GetLength(0) / Constants.ChunkSize);
            var chunkHeight = Mathf.FloorToInt(cellsToRender.GetLength(1) / Constants.ChunkSize);

            _chunkRenderers = new ChunkRenderer[chunkWidth, chunkHeight];
            for (int x = 0; x < chunkWidth; x++)
            {
                for (int z = 0; z < chunkHeight; z++)
                {
                    _chunkRenderers[x, z] = MakeChunkRenderer(x, z);
                }
            }

            _pathfinder = CreatePathfinder();
        }

        internal Cell[,] GetCells(int offsetX, int offsetY)
        {
            var cells = new Cell[Constants.ChunkSize, Constants.ChunkSize];
            offsetX *= Constants.ChunkSize;
            offsetY *= Constants.ChunkSize;

            for (var x = 0; x < Constants.ChunkSize; x++)
            {
                for (var y = 0; y < Constants.ChunkSize; y++)
                {
                    cells[x, y] = _cells[offsetX + x, offsetY + y];
                }
            }

            return cells;
        }

        private Pathfinder CreatePathfinder()
        {
            var pf = new GameObject("Pathfinder");
            pf.transform.SetParent(transform);
            return pf.AddComponent<Pathfinder>();
        }

    

        private void IndexCells(Cell[,] cells)
        {
            _cellLookup = new Dictionary<ICoord, Cell>();
            foreach (var cell in cells)
            {
                _cellLookup.Add(cell.Coord, cell);
            }
        }

        private ChunkRenderer MakeChunkRenderer(int x, int z)
        {
            var renderer = _chunkRendererFactory.CreateChunkRenderer(x, z, GetCells(x, z));
            renderer.transform.SetParent(transform);
            renderer.name = $"{x} - {z}";

            renderer.SetMaterial(ChunkMaterial);

            return renderer;
        }
    }
}