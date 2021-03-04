using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Assets.Map
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class ChunkRenderer : MonoBehaviour
    {
        private const int _vertsPerCell = 4;

        private bool _active = true;
        private Camera _camera;

        private Cell[,] _cells;

        private List<Color> _colors;

        private GridMesh _gridMesh;
        private int _maxX = -int.MaxValue;
        private int _maxZ = -int.MaxValue;
        private Mesh _mesh;

        private MeshCollider _meshCollider;

        private int _minX = int.MaxValue;
        private int _minZ = int.MaxValue;
        private bool _redraw;
        private List<int> _triangles;

        private List<Vector2> _uvs;

        private List<Vector3> _vertices;
        public int X { get; internal set; }

        public int Z { get; internal set; }

        public void Activate(bool force = false)
        {
            if (!_active || force)
            {
                gameObject.SetActive(true);
                MapEventManager.ChunkRendererActivated(this);
                _active = true;
            }
        }

        public void AddTriangle(int a, int b, int c)
        {
            _triangles.Add(a);
            _triangles.Add(b);
            _triangles.Add(c);
        }

        public bool CoordInChunk(Coord coord)
        {
            var maxx = (X + 1) * Constants.ChunkSize;
            var minx = X * Constants.ChunkSize;

            var maxz = (Z + 1) * Constants.ChunkSize;
            var minz = Z * Constants.ChunkSize;

            return (coord.X <= maxx) && (coord.X >= minx) &&
                   (coord.Z <= maxz) && (coord.Z >= minz);
        }

        public void Deactivate()
        {
            if (_active)
            {
                gameObject.SetActive(false);
                MapEventManager.ChunkRendererDeactivated(this);
                _active = false;
            }
        }

        public void GenerateMesh()
        {
            ResetMesh();

            var width = GetWidth();

            GenerateInternalMap(width);

            FillSideRenderWalls(width);

            AssignMesh();
        }

        public void OnMouseOver()
        {
            var inputRay = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(inputRay, out RaycastHit hit))
            {
                var hitX = (int)hit.point.x % Constants.ChunkSize;
                var hitZ = (int)hit.point.z % Constants.ChunkSize;
                var cell = _cells[hitX, hitZ];

                CellEventManager.MouseOverCell(cell);
            }
        }

        public void OnMouseUp()
        {
            var inputRay = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(inputRay, out RaycastHit hit))
            {
                var hitX = (int)hit.point.x % Constants.ChunkSize;
                var hitZ = (int)hit.point.z % Constants.ChunkSize;
                var cell = _cells[hitX, hitZ];

                CellEventManager.CellClicked(cell);
            }
        }

        public void SetCamera(Camera camera)
        {
            _camera = camera;
        }

        public void SetCells(Cell[,] cells)
        {
            _cells = cells;

            for (var x = 0; x < _cells.GetLength(0); x++)
            {
                for (var z = 0; z < _cells.GetLength(1); z++)
                {
                    var cell = _cells[x, z];

                    if (cell.Coord.X > _maxX)
                    {
                        _maxX = cell.Coord.X;
                    }

                    if (cell.Coord.X < _minX)
                    {
                        _minX = cell.Coord.X;
                    }

                    if (cell.Coord.Z > _maxZ)
                    {
                        _maxZ = cell.Coord.Z;
                    }

                    if (cell.Coord.Z < _minZ)
                    {
                        _minZ = cell.Coord.Z;
                    }
                }
            }
        }

        public void Start()
        {
            transform.position = GetPosition();
            GenerateMesh();
            GenerateGrid();
        }

        public void Update()
        {
            if (_redraw)
            {
                GenerateMesh();
                _redraw = false;
            }
        }

        internal void SetMaterial(Material chunkMaterial)
        {
            GetComponent<MeshRenderer>().materials = new Material[] { chunkMaterial };
        }

        private void AddBackWall(int width, int lastVert, int z, int x)
        {
            if (x == width && z < width)
            {
                if (z == 0)
                {
                    AddTriangle((width * _vertsPerCell) - 3, lastVert + width + 2, lastVert + width);
                    AddTriangle((width * _vertsPerCell) - 3, (width * _vertsPerCell) - 2, lastVert + width + 2);
                }
                else
                {
                    var zVert = lastVert + width + (z * 2);
                    var tVert = (z * width * _vertsPerCell) + (width * _vertsPerCell) - 3;
                    AddTriangle(tVert, tVert + 1, zVert);
                    if (z < width - 1)
                    {
                        AddTriangle(tVert + 1, zVert + 2, zVert);
                    }
                    else
                    {
                        AddTriangle(tVert + 1, zVert + width + 1, zVert);
                    }
                }
            }
        }

        private void AddFrontWall(int width, int lastVert, int z, int x)
        {
            if (z == 0 && x < width)
            {
                AddTriangle(x * _vertsPerCell, (x * _vertsPerCell) + 1, x + lastVert);
                AddTriangle((x * _vertsPerCell) + 1, x + lastVert + 1, x + lastVert);
            }
        }

        private void AddLeftWall(int width, int lastVert, int z, int x)
        {
            if (x == 0 && z < width)
            {
                if (z == 0)
                {
                    AddTriangle((z * width * _vertsPerCell) + 3, z * width * _vertsPerCell, lastVert);
                    AddTriangle(lastVert, lastVert + width + 1, (z * width * _vertsPerCell) + 3);
                }
                else
                {
                    var zVert = lastVert + (width - 1) + (z * 2);

                    AddTriangle((z * width * _vertsPerCell) + 3, z * width * _vertsPerCell, zVert);
                    AddTriangle(zVert, zVert + 2, (z * width * _vertsPerCell) + 3);
                }
            }
        }

        private void AddLowerVert(int width, int z, int x, Color color)
        {
            if (z == 0 || z == width || x == 0 || x == width)
            {
                AddVert(x - 0.5f, -1f, z - 0.5f, color);
            }
        }

        private void AddRightWall(int width, int lastVert, int z, int x)
        {
            if (z == width && x < width)
            {
                var zVert = (z * (width - 1) * _vertsPerCell) + (x * _vertsPerCell) + 2;
                var xvert = lastVert + (width - 1) + (z * 2) + x;
                AddTriangle(zVert, zVert + 1, xvert);
                AddTriangle(xvert, xvert + 1, zVert);
            }
        }

        private void AddVert(float x, float y, float z, Color color)
        {
            _vertices.Add(new Vector3(x, y, z));
            var uvx = Mathf.Max(0, x / GetWidth());
            var uvz = Mathf.Max(0, z / GetWidth());
            _uvs.Add(new Vector2(uvx, uvz));
            _colors.Add(color);
        }

        private void AssignMesh()
        {
            _mesh.vertices = _vertices.ToArray();
            _mesh.colors = _colors.ToArray();
            _mesh.triangles = _triangles.ToArray();
            _mesh.uv = _uvs.ToArray();

            SetAllNormalsToUp();
            _meshCollider.sharedMesh = _mesh;
        }

        private void Awake()
        {
            GetComponent<MeshFilter>().mesh = _mesh = new Mesh();
            _mesh.name = $"Mesh {name}";
            _meshCollider = gameObject.AddComponent<MeshCollider>();

            MapEventManager.OnCellTerrainChangedDelegate += CellTerrainChanged;
        }

        private bool CellIsInChunk(Cell cell)
        {
            var x = cell.Coord.X;
            var z = cell.Coord.Z;

            if (x < _maxX && x >= _minX && z < _maxZ && z >= _minZ)
            {
                return true;
            }
            return false;
        }

        private void CellTerrainChanged(Cell cell, ITerrain terrain)
        {
            if (CellIsInChunk(cell))
            {
                _redraw = true;
            }
        }

        private void FillSideRenderWalls(int width)
        {
            var lastVert = _vertices.Count;
            var lastColor = Color.magenta;
            for (var z = 0; z <= width; z++)
            {
                for (var x = 0; x <= width; x++)
                {
                    var color = lastColor;
                    if (x != width && z != width)
                    {
                        var cell = _cells[x, z];
                        color = cell.Terrain.Color;
                        lastColor = color;
                    }

                    AddLowerVert(width, z, x, color);
                    AddFrontWall(width, lastVert, z, x);
                    AddLeftWall(width, lastVert, z, x);
                    AddBackWall(width, lastVert, z, x);
                    AddRightWall(width, lastVert, z, x);
                }
            }
        }

        private void GenerateGrid()
        {
            var gridObj = new GameObject("Grid");
            gridObj.transform.SetParent(transform);
            gridObj.transform.localPosition = new Vector3(-0.5f, 0.01f, -0.5f);
            gridObj.AddComponent<MeshFilter>();
            var renderer = gridObj.AddComponent<MeshRenderer>();
            renderer.shadowCastingMode = ShadowCastingMode.Off;
            _gridMesh = gridObj.AddComponent<GridMesh>();

            var mat = new Material(Shader.Find("Sprites/Default"))
            {
                color = new Color(0, 0, 0, 0.2f)
            };

            _gridMesh.DrawGrid(GetWidth(), mat);
        }

        private void GenerateInternalMap(int width)
        {
            var i = 0;
            for (var z = 0; z < width; z++)
            {
                for (var x = 0; x < width; x++)
                {
                    var cell = _cells[x, z];
                    var height = cell.Coord.Y;

                    var color = cell.Terrain.Color;

                    AddVert(x - 0.5f, height, z - 0.5f, color);
                    AddVert(x + 0.5f, height, z - 0.5f, color);
                    AddVert(x + 0.5f, height, z + 0.5f, color);
                    AddVert(x - 0.5f, height, z + 0.5f, color);

                    var c = i * _vertsPerCell;
                    AddTriangle(c + 3, c + 2, c + 1);
                    AddTriangle(c + 1, c, c + 3);

                    if (x < width - 1)
                    {
                        AddTriangle(c + 1, c + 2, c + 4);
                        AddTriangle(c + 4, c + 2, c + 7);
                    }

                    var cz = width * 4;
                    if (z < width - 1)
                    {
                        AddTriangle(c + 2, c + 3, c + cz + 1);
                        AddTriangle(c + cz + 1, c + 3, c + cz);
                    }

                    i++;
                }
            }
        }

        private Vector3 GetPosition()
        {
            return new Vector3((X * Constants.ChunkSize) + 0.5f, 0, Z * Constants.ChunkSize + 0.5f);
        }

        private int GetWidth()
        {
            return Constants.ChunkSize;
        }

        private void OnDrawGizmos()
        {
            //foreach (var cell in _cells)
            //{
            //    var resources = cell.Terrain.ResourceValue;
            //    var off = 0.4f;
            //    foreach (var res in resources)
            //    {
            //        switch (res.Key)
            //        {
            //            case Resources.ResourceType.Food:
            //                Gizmos.color = Color.red;
            //                break;

            //            case Resources.ResourceType.Wood:
            //                Gizmos.color = Color.green;
            //                break;

            //            case Resources.ResourceType.Stone:
            //                Gizmos.color = Color.gray;
            //                break;
            //        }

            //        Gizmos.DrawCube(new Vector3(cell.Coord.X + off, cell.Coord.Y + (0.1f * res.Value), cell.Coord.Z + 0.5f), new Vector3(0.1f, 0.1f * res.Value, 0.1f));
            //        off += 0.1f;
            //    }
            //}
        }

        private void ResetMesh()
        {
            _mesh.Clear();
            _uvs = new List<Vector2>();
            _vertices = new List<Vector3>();
            _colors = new List<Color>();
            _triangles = new List<int>();
        }

        private void SetAllNormalsToUp()
        {
            var normals = new Vector3[_vertices.Count];
            for (int v = 0; v < _vertices.Count; v++)
            {
                normals[v] = Vector3.up;
            }
            _mesh.SetNormals(normals);
        }
    }
}