using Assets.Helpers;
using Assets.Map;
using Assets.ServiceLocator;
using Assets.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Assets
{
    public class SpawnManager : LocatableMonoBehaviorBase, ISpawnManager
    {
        public Material InvalidPreviewMaterial;
        public Material PreviewMaterial;

        private readonly List<ChunkRenderer> _activeChunks = new List<ChunkRenderer>();
        private readonly Dictionary<ChunkRenderer, List<IStructure>> _chunkStructureLookup = new Dictionary<ChunkRenderer, List<IStructure>>();
        private readonly List<GameObject> _destroyCache = new List<GameObject>();
        private readonly Dictionary<GameObject, Material[]> _materialBackup = new Dictionary<GameObject, Material[]>();
        private readonly Dictionary<string, Queue<GameObject>> _objectPools = new Dictionary<string, Queue<GameObject>>();
        private readonly Dictionary<IStructure, GameObject> _structureObjectLookup = new Dictionary<IStructure, GameObject>();
        private readonly Dictionary<StructureType, string> _typeAssetLookup = new Dictionary<StructureType, string>();

        private Dictionary<GameObject, Outline> _outlineLookup = new Dictionary<GameObject, Outline>();

        private List<GameObject> _outlinesToDisable = new List<GameObject>();

        private List<GameObject> _outlinesToEnable = new List<GameObject>();

        public delegate void SpawnCallback(GameObject spawnedObject);

        public void AddItemToDestroy(GameObject gameObject)
        {
            lock (_destroyCache)
            {
                _destroyCache.Add(gameObject);
            }
        }

        public void DestroyItemsInCache()
        {
            try
            {
                lock (_destroyCache)
                {
                    while (_destroyCache.Count > 0)
                    {
                        var item = _destroyCache[0];
                        _destroyCache.RemoveAt(0);
                        if (item != null)
                        {
                            Destroy(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log($"Destroy failed: {ex}");
            }
        }

        public override void Initialize()
        {
            var poolConfigs = new List<(string pool, int size)>
            {
                ("Tree", 1000),
                ("Rock", 500),
                ("Road", 1000),
                ("BellTower", 10),
                ("Barn", 100),
                ("House", 200),
                ("Field", 500),
                ("Empty", 5),
            };

            foreach (var (pool, size) in poolConfigs)
            {
                var queue = new Queue<GameObject>();
                for (var i = 0; i < size; i++)
                {
                    SpawnModel(pool, Vector3.zero, (obj) =>
                    {
                        _objectPools[pool].Enqueue(obj);
                        InitOutline(obj);
                        obj.SetActive(false);
                    });
                }

                _objectPools.Add(pool, queue);
            }

            _typeAssetLookup.Add(StructureType.Tree, "Tree");
            _typeAssetLookup.Add(StructureType.Rock, "Rock");
            _typeAssetLookup.Add(StructureType.Core, "BellTower");
            _typeAssetLookup.Add(StructureType.Road, "Road");
            _typeAssetLookup.Add(StructureType.House, "House");
            _typeAssetLookup.Add(StructureType.Barn, "Barn");
            _typeAssetLookup.Add(StructureType.Field, "Field");
            _typeAssetLookup.Add(StructureType.Empty, "Empty");

            MapEventManager.OnChunkRenderCreated += MapEventManager_OnChunkRenderCreated;
            MapEventManager.OnChunkRenderActivated += MapEventManager_OnChunkRenderActivated;
            MapEventManager.OnChunkRenderDeactivated += MapEventManager_OnChunkRenderDeactivated;

            StructureEventManager.OnStructurePlanned += StructureEventManager_OnStructurePlanned;
            StructureEventManager.OnStructureDestroyed += StructureEventManager_OnStructureDestroyed;

            StructureEventManager.OnShowHighlight += (structure) => SetOutlineOnStructure(structure, true);
            StructureEventManager.OnHideHighlight += (structure) => SetOutlineOnStructure(structure, false);
        }

        public void RecyleItem(string pool, GameObject gameObject)
        {
            if (gameObject == null)
            {
                return;
            }
            if (_objectPools.ContainsKey(pool))
            {
                RestoreMaterials(gameObject);

                _objectPools[pool].Enqueue(gameObject);
                gameObject.SetActive(false);
            }
            else
            {
                AddItemToDestroy(gameObject);
            }
        }

        public void RecyleItem(StructureType type, GameObject gameObject)
        {
            RecyleItem(type.ToString(), gameObject);
        }

        public void SetOutlineOnStructure(IStructure structure, bool active)
        {
            if (_structureObjectLookup.ContainsKey(structure))
            {
                var obj = _structureObjectLookup[structure];

                if (obj != null)
                {
                    if (active)
                    {
                        lock (_outlinesToEnable)
                        {
                            _outlinesToEnable.Add(obj);
                        }
                    }
                    else
                    {
                        lock (_outlinesToDisable)
                        {
                            _outlinesToDisable.Add(obj);
                        }
                    }
                }
            }
        }

        public void SpawnModel(string address, Vector3 position, SpawnCallback callback)
        {
            if (!_objectPools.ContainsKey(address))
            {
                var op = Addressables.InstantiateAsync(address, position, Quaternion.identity, transform);
                op.Completed += (AsyncOperationHandle<GameObject> obj) => callback.Invoke(obj.Result);
            }
            else
            {
                var obj = ActivatePoolObject(address, position, transform);
                callback.Invoke(obj);
            }
        }

        public void SpawnModel(StructureType type, Vector3 position, SpawnCallback callback)
        {
            SpawnModel(GetAssetNameForStructureType(type), position, callback);
        }

        public void SpawnPreviewModel(string meshName, Vector3 position, bool valid, SpawnCallback callback)
        {
            SpawnModel(meshName, position, (obj) =>
            {
                BackupMaterials(obj);
                obj.GetComponent<MeshRenderer>().SetAllMaterial(valid ? PreviewMaterial : InvalidPreviewMaterial);
                callback.Invoke(obj);
            });
        }

        public void SpawnPreviewModel(StructureType type, Vector3 position, bool valid, SpawnCallback callback)
        {
            SpawnPreviewModel(GetAssetNameForStructureType(type), position, valid, callback);
        }

        public void SpawnUIElement(string name, Transform parent, SpawnCallback callback)
        {
            var op = Addressables.InstantiateAsync(name, parent.transform.position, Quaternion.identity, parent);
            op.Completed += (AsyncOperationHandle<GameObject> obj) => callback.Invoke(obj.Result);
        }

        public void Update()
        {
            DestroyItemsInCache();

            UpdateHighlights();
        }

        private GameObject ActivatePoolObject(string address, Vector3 position, Transform parent)
        {
            var obj = _objectPools[address].Dequeue();
            obj.transform.parent = parent;
            obj.transform.position = position;
            obj.SetActive(true);

            _objectPools[address].Enqueue(obj);
            return obj;
        }

        private void BackupMaterials(GameObject go)
        {
            if (!_materialBackup.ContainsKey(go))
            {
                _materialBackup.Add(go, go.GetComponent<MeshRenderer>().materials);
            }
        }

        private string GetAssetNameForStructureType(StructureType type)
        {
            return _typeAssetLookup[type];
        }

        private void InitOutline(GameObject obj)
        {
            var outline = obj.AddComponent<Outline>();

            outline.OutlineMode = Outline.Mode.OutlineVisible;
            outline.OutlineColor = Color.red;
            outline.OutlineWidth = 5f;

            outline.enabled = false;

            _outlineLookup.Add(obj, outline);
        }

        private void MapEventManager_OnChunkRenderActivated(ChunkRenderer renderer)
        {
            if (!_activeChunks.Contains(renderer))
            {
                Debug.Log($"Activate chunk {renderer.X}:{renderer.Z}");
                foreach (var structure in _chunkStructureLookup[renderer])
                {
                    SpawnStructure(structure);
                }
                _activeChunks.Add(renderer);
            }
        }

        private void MapEventManager_OnChunkRenderCreated(ChunkRenderer renderer)
        {
            _chunkStructureLookup.Add(renderer, new List<IStructure>());
        }

        private void MapEventManager_OnChunkRenderDeactivated(ChunkRenderer renderer)
        {
            if (_activeChunks.Contains(renderer))
            {
                Debug.Log($"Deactivate chunk {renderer.X}:{renderer.Z}");
                _activeChunks.Remove(renderer);
            }
        }

        private void RestoreMaterials(GameObject go)
        {
            if (_materialBackup.ContainsKey(go))
            {
                go.GetComponent<MeshRenderer>().SetMeshMaterial(_materialBackup[go]);
                _materialBackup.Remove(go);
            }
        }

        private void SpawnStructure(IStructure structure)
        {
            // never invoke this directly, this should invoke when the spawn manager decides its required
            var placement = StructureExtensions.CalculatePlacementPosition(structure.GetOrigin(), structure.Width, structure.Height);
            var address = GetAssetNameForStructureType(structure.Type);
            SpawnModel(address, placement, (obj) =>
            {
                if (!_structureObjectLookup.ContainsKey(structure))
                {
                    _structureObjectLookup.Add(structure, obj);
                }
                _structureObjectLookup[structure] = obj;
            });
        }

        private void StructureEventManager_OnStructureDestroyed(IStructure structure)
        {
            var renderer = _chunkStructureLookup.Keys.First(c => c.CoordInChunk(structure.GetOrigin()));
            _chunkStructureLookup[renderer].Remove(structure);

            var pool = GetAssetNameForStructureType(structure.Type);

            if (_structureObjectLookup.ContainsKey(structure))
            {
                RecyleItem(pool, _structureObjectLookup[structure]);
            }
        }

        private void StructureEventManager_OnStructurePlanned(IStructure structure)
        {
            var drawnOnce = false; // structures can be in more than one chunk, just to be sure we do not redraw it a lot
            foreach (var coord in structure.OccupiedCoords)
            {
                var renderer = _chunkStructureLookup.Keys.First(c => c.CoordInChunk(coord));

                var chunkStructures = _chunkStructureLookup[renderer];
                if (!chunkStructures.Contains(structure))
                {
                    chunkStructures.Add(structure);
                }

                // draw this structure if the current chunk is active,
                // otherwise it will draw when the chunk activates later
                if (_activeChunks.Contains(renderer) && !drawnOnce)
                {
                    SpawnStructure(structure);
                    drawnOnce = true;
                }
            }
        }

        private void UpdateHighlights()
        {
            lock (_outlinesToEnable)
            {
                foreach (var outline in _outlinesToEnable)
                {
                    _outlineLookup[outline].enabled = true;
                }
                _outlinesToEnable.Clear();
            }

            lock (_outlinesToDisable)
            {
                foreach (var outline in _outlinesToDisable)
                {
                    _outlineLookup[outline].enabled = false;
                }
                _outlinesToDisable.Clear();
            }
        }
    }
}