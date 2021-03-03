﻿using Assets.Helpers;
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
        private readonly Dictionary<string, List<GameObject>> _objectPools = new Dictionary<string, List<GameObject>>();
        private readonly Dictionary<IStructure, GameObject> _structureObjectLookup = new Dictionary<IStructure, GameObject>();

        private Dictionary<GameObject, Outline> _outlineLookup = new Dictionary<GameObject, Outline>();

        private List<GameObject> _outlinesToDisable = new List<GameObject>();

        private List<GameObject> _outlinesToEnable = new List<GameObject>();
        private IStructureDefinitionManager _structureDefinitionManager;

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
            _structureDefinitionManager = Locate<IStructureDefinitionManager>();

            var pools = new List<(string pool, int size)>
            {
                ("RemoveMarker", 50),
            };

            foreach (var def in _structureDefinitionManager.StructureDefinitions)
            {
                pools.Add((def.Asset, def.PoolSize));
            }

            foreach (var (pool, size) in pools)
            {
                Debug.Log($"Init {pool}");

                var queue = new List<GameObject>();
                _objectPools.Add(pool, queue);
                for (var i = 0; i < size; i++)
                {
                    InitModel(pool, Vector3.zero, (obj) =>
                    {
                        _objectPools[pool].Add(obj);
                        obj.name = $"{pool}-{_objectPools[pool].Count}";
                        InitOutline(obj);
                        obj.SetActive(false);
                    });
                }
            }

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

                _outlineLookup[gameObject].enabled = false;

                // add recyled items to front of list so they are used first when new items are required
                _objectPools[pool].Remove(gameObject);
                _objectPools[pool].Insert(0, gameObject);
                gameObject.SetActive(false);
            }
            else
            {
                AddItemToDestroy(gameObject);
            }
        }

        public void RecyleItem(StructureDefinition.StructureType type, GameObject gameObject)
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

        private void InitModel(string address, Vector3 position, SpawnCallback callback)
        {
            var op = Addressables.InstantiateAsync(address, position, Quaternion.identity, transform);
            op.Completed += (AsyncOperationHandle<GameObject> obj) => callback.Invoke(obj.Result);
        }

        public void SpawnModel(string address, Vector3 position, SpawnCallback callback)
        {
            var obj = ActivatePoolObject(address, position, transform);
            callback.Invoke(obj);
        }

        public void SpawnModel(StructureDefinition.StructureType type, Vector3 position, SpawnCallback callback)
        {
            SpawnModel(_structureDefinitionManager.GetAssetNameForStructureType(type), position, callback);
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

        public void SpawnPreviewModel(StructureDefinition.StructureType type, Vector3 position, bool valid, SpawnCallback callback)
        {
            SpawnPreviewModel(_structureDefinitionManager.GetAssetNameForStructureType(type), position, valid, callback);
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

            SpawnPendingStructures();
        }

        private void SpawnPendingStructures()
        {
            foreach (var structure in _structuresToSpawn)
            {
                SpawnStructure(structure);
            }
            _structuresToSpawn.Clear();
        }

        private GameObject ActivatePoolObject(string address, Vector3 position, Transform parent)
        {
            var obj = _objectPools[address][0];
            obj.transform.parent = parent;
            obj.transform.position = position;
            obj.SetActive(true);

            _objectPools[address].Remove(obj);
            _objectPools[address].Add(obj);
            return obj;
        }

        private void BackupMaterials(GameObject go)
        {
            if (!_materialBackup.ContainsKey(go))
            {
                _materialBackup.Add(go, go.GetComponent<MeshRenderer>().materials);
            }
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
            var placement = structure.Coord.ToAdjustedVector3();
            var address = _structureDefinitionManager.GetAssetNameForStructureType(structure.Type);
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
            var renderer = _chunkStructureLookup.Keys.First(c => c.CoordInChunk(structure.Coord));
            _chunkStructureLookup[renderer].Remove(structure);

            var pool = _structureDefinitionManager.GetAssetNameForStructureType(structure.Type);

            if (_structureObjectLookup.ContainsKey(structure))
            {
                RecyleItem(pool, _structureObjectLookup[structure]);
            }
        }

        private void StructureEventManager_OnStructurePlanned(IStructure structure)
        {
            var drawnOnce = false; // structures can be in more than one chunk, just to be sure we do not redraw it a lot

            var renderer = _chunkStructureLookup.Keys.First(c => c.CoordInChunk(structure.Coord));

            var chunkStructures = _chunkStructureLookup[renderer];
            if (!chunkStructures.Contains(structure))
            {
                chunkStructures.Add(structure);
            }

            // draw this structure if the current chunk is active,
            // otherwise it will draw when the chunk activates later
            if (_activeChunks.Contains(renderer) && !drawnOnce)
            {
                _structuresToSpawn.Add(structure);
                drawnOnce = true;
            }
        }

        private List<IStructure> _structuresToSpawn = new List<IStructure>();

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