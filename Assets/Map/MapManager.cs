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

        private Dictionary<(int x, int z), Cell> _cellLookup;
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
                    var renderer = MakeChunkRenderer(x, z);
                    _chunkRenderers[x, z] = renderer;

                    MapEventManager.ChunkRenderCreated(renderer);

                    renderer.Deactivate();
                }
            }

            _pathfinder = CreatePathfinder();
            LinkCellsToNeighbors(_cells, Width, Height);
        }

        public void DestroyChunks()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

        public bool TryGetCellAtCoord(Coord coord, out Cell cell)
        {
            return TryGetCellAtCoord(coord.X, coord.Z, out cell);
        }

        public bool TryGetCellAtCoord(int x, int z, out Cell cell)
        {
            cell = null;
            if (x >= Width || x < 0 || z >= Height || z < 0)
            {
                return false;
            }
            cell = _cellLookup[(x, z)];
            return true;
        }

        public Cell GetCenter()
        {
            return _cells[Width / 2, Height / 2];
        }

        public List<Cell> GetCircle(Coord center, int radius)
        {
            var cells = new List<Cell>();

            var centerX = center.X;
            var centerY = center.Z;

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

            CameraEventManager.OnCameraPositionChanged += ActivateChunksNearCamera;
        }

        private float _lastZoomLevel = -1;
        private ChunkRenderer _lastActiveChunk;

        private void ActivateChunksNearCamera(Vector3 cameraLocation)
        {
            if (_lastActiveChunk == null || _lastZoomLevel == -1)
            {
                TryGetCellAtCoord((int)cameraLocation.x, (int)cameraLocation.z, out Cell cell);
                _lastActiveChunk = GetChunkForcell(cell);
                _lastZoomLevel = -1;
                ActivateChunks(_lastActiveChunk, cameraLocation.y);
            }
            else
            {
                TryGetCellAtCoord((int)cameraLocation.x, (int)cameraLocation.z, out Cell cell);
                var currentChunk = GetChunkForcell(cell);
                if (_lastActiveChunk != currentChunk || _lastZoomLevel != cameraLocation.y)
                {
                    ActivateChunks(currentChunk, cameraLocation.y);
                }
                _lastActiveChunk = currentChunk;
            }
            _lastZoomLevel = cameraLocation.y;
        }

        private void ActivateChunks(ChunkRenderer currentChunk, float zoom)
        {
            var offset = 1;
            var chunkWidth = Mathf.FloorToInt(_cells.GetLength(0) / Constants.ChunkSize) - 1;
            var chunkHeight = Mathf.FloorToInt(_cells.GetLength(1) / Constants.ChunkSize) - 1;

            var xstart = Mathf.Max(0, currentChunk.X - offset);
            var xend = Mathf.Min(chunkWidth, currentChunk.X + offset);
            var zstart = Mathf.Max(0, currentChunk.Z - offset);
            var zend = Mathf.Min(chunkHeight, currentChunk.Z + offset);

            var p = new string[12];
            try
            {
                var deactivate = _chunkRenderers.Flatten().ToList();
                for (var x = xstart; x <= xend; x++)
                {
                    for (var z = zstart; z <= zend; z++)
                    {
                        var chunk = _chunkRenderers[x, z];

                        deactivate.Remove(chunk);
                        if (chunk == currentChunk)
                        {
                            // activate current chunk last
                            continue;
                        }
                        chunk.Activate();
                    }
                }

                foreach (var cr in deactivate)
                {
                    cr.Deactivate();
                }

                currentChunk.Activate(true);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"xstart: {xstart}");
                Debug.LogError($"xend: {xend}");
                Debug.LogError($"zstart: {zstart}");
                Debug.LogError($"zend: {zend}");
            }
        }

        internal ChunkRenderer GetChunkForcell(Cell cell)
        {
            var chunkX = cell.Coord.X / Constants.ChunkSize;
            var chunkZ = cell.Coord.Z / Constants.ChunkSize;

            return _chunkRenderers[chunkX, chunkZ];
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

        internal void LinkCellsToNeighbors(Cell[,] cells, int width, int height)
        {
            for (var z = 0; z < height; z++)
            {
                for (var x = 0; x < width; x++)
                {
                    var cell = cells[x, z];
                    if (x > 0)
                    {
                        cell.SetNeighbor(Direction.W, cells[x - 1, z]);

                        if (z > 0)
                        {
                            cell.SetNeighbor(Direction.SW, cells[x - 1, z - 1]);

                            if (x < width - 1)
                            {
                                cell.SetNeighbor(Direction.SE, cells[x + 1, z - 1]);
                            }
                        }
                    }

                    if (z > 0)
                    {
                        cell.SetNeighbor(Direction.S, cells[x, z - 1]);
                    }
                }
            }
        }

        private Pathfinder CreatePathfinder()
        {
            var pf = new GameObject("Pathfinder");
            pf.transform.SetParent(transform);
            return pf.AddComponent<Pathfinder>();
        }

        private void IndexCells(Cell[,] cells)
        {
            _cellLookup = new Dictionary<(int x, int z), Cell>();
            foreach (var cell in cells)
            {
                _cellLookup.Add((cell.Coord.X, cell.Coord.Z), cell);
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

        public List<Cell> GetRectangle(Coord coord, int width, int height)
        {
            var xstart = Mathf.Min(coord.X, coord.X + width);
            var xend = Mathf.Max(coord.X, coord.X + width);
            var zstart = Mathf.Min(coord.Z, coord.Z + width);
            var zend = Mathf.Max(coord.Z, coord.Z + width);

            var cells = new List<Cell>();
            for (var x = xstart; x < xend; x++)
            {
                for (var z = zstart; z < zend; z++)
                {
                    if (TryGetCellAtCoord(x, z, out Cell cell))
                    {
                        cells.Add(cell);
                    }
                }
            }

            return cells;
        }
    }
}