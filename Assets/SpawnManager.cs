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
        private readonly Dictionary<ChunkRenderer, List<IStructure>> _chunkStructureLookup = new Dictionary<ChunkRenderer, List<IStructure>>();
        private readonly List<GameObject> _destroyCache = new List<GameObject>();

        private Dictionary<string, Queue<GameObject>> _objectPools = new Dictionary<string, Queue<GameObject>>();

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
                ("Road", 100),
                ("BellTower", 10),
                ("Barn", 20),
                ("House", 100),
            };

            foreach (var (pool, size) in poolConfigs)
            {
                var queue = new Queue<GameObject>();
                for (var i = 0; i < size; i++)
                {
                    SpawnModel(pool, Vector3.zero, (obj) =>
                    {
                        _objectPools[pool].Enqueue(obj);
                        obj.SetActive(false);
                    });
                }

                _objectPools.Add(pool, queue);
            }

            MapEventManager.OnChunkRenderCreated += MapEventManager_OnChunkRenderCreated;
            MapEventManager.OnChunkRenderActivated += MapEventManager_OnChunkRenderActivated;
            MapEventManager.OnChunkRenderDeactivated += MapEventManager_OnChunkRenderDeactivated;

            StructureEventManager.OnStructurePlanned += StructureEventManager_OnStructurePlanned;
            StructureEventManager.OnStructureDestroyed += StructureEventManager_OnStructureDestroyed;
        }

        public void RecyleItem(string pool, GameObject gameObject)
        {
            if (gameObject == null)
            {
                return;
            }
            if (_objectPools.ContainsKey(pool))
            {
                _objectPools[pool].Enqueue(gameObject);
            }
            else
            {
                AddItemToDestroy(gameObject);
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

        public void SpawnUIElement(string name, Transform parent, SpawnCallback callback)
        {
            var op = Addressables.InstantiateAsync(name, parent.transform.position, Quaternion.identity, parent);
            op.Completed += (AsyncOperationHandle<GameObject> obj) => callback.Invoke(obj.Result);
        }

        public void Update()
        {
            DestroyItemsInCache();
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

        private List<ChunkRenderer> _activeChunks = new List<ChunkRenderer>();

        private void MapEventManager_OnChunkRenderActivated(ChunkRenderer renderer)
        {
            foreach (var structure in _chunkStructureLookup[renderer])
            {
                SpawnStructure(structure);
            }

            if (!_activeChunks.Contains(renderer))
            {
                _activeChunks.Add(renderer);
            }
        }

        private void SpawnStructure(IStructure structure)
        {
            // never invoke this directly, this should invoke when the spawn manager decides its required
            var placement = StructureExtensions.CalculatePlacementPosition(structure.GetOrigin(), structure.Width, structure.Height);
            SpawnModel(structure.Address, placement, (obj) =>
            {
                if (!_structureObjectLookup.ContainsKey(structure))
                {
                    _structureObjectLookup.Add(structure, obj);
                }
                _structureObjectLookup[structure] = obj;
            });
        }

        private void MapEventManager_OnChunkRenderCreated(ChunkRenderer renderer)
        {
            _chunkStructureLookup.Add(renderer, new List<IStructure>());
        }

        private void MapEventManager_OnChunkRenderDeactivated(ChunkRenderer renderer)
        {
            if (_activeChunks.Contains(renderer))
            {
                _activeChunks.Remove(renderer);
            }
        }

        private Dictionary<IStructure, GameObject> _structureObjectLookup = new Dictionary<IStructure, GameObject>();

        private void StructureEventManager_OnStructureDestroyed(IStructure structure)
        {
            var renderer = _chunkStructureLookup.Keys.First(c => c.CoordInChunk(structure.GetOrigin()));
            _chunkStructureLookup[renderer].Remove(structure);

            RecyleItem(structure.Address, _structureObjectLookup[structure]);
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
    }
}