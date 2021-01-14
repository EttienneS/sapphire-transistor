using Assets.ServiceLocator;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Assets
{
    public class SpawnManager : LocatableMonoBehaviorBase, ISpawnManager
    {
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
                ("Tree", 200)
            };

            foreach (var (pool, size) in poolConfigs)
            {
                var queue = new Queue<GameObject>();
                for (var i = 0; i < size; i++)
                {
                    SpawnAddressable(pool, Vector3.zero, (obj) =>
                    {
                        _objectPools[pool].Enqueue(obj);
                        obj.SetActive(false);
                    });
                }

                _objectPools.Add(pool, queue);
            }
        }

        public void RecyleItem(string pool, GameObject gameObject)
        {
            if (_objectPools.ContainsKey(pool))
            {
                _objectPools[pool].Enqueue(gameObject);
            }
            else
            {
                AddItemToDestroy(gameObject);
            }
        }

        public void SpawnAddressable(string address, Vector3 position, Transform parent, SpawnCallback callback)
        {
            if (!_objectPools.ContainsKey(address))
            {
                var op = Addressables.InstantiateAsync(address, position, Quaternion.identity, parent);
                op.Completed += (AsyncOperationHandle<GameObject> obj) => callback.Invoke(obj.Result);
            }
            else
            {
                var obj = ActivatePoolObject(address, position, parent);
                callback.Invoke(obj);
            }
        }

        public void SpawnAddressable(string address, Vector3 position, SpawnCallback callback)
        {
            SpawnAddressable(address, position, transform, callback);
        }

        public void SpawnUIElement(string name, Transform parent, SpawnCallback callback)
        {
            SpawnAddressable(name, parent.transform.position, parent, callback);
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
    }
}