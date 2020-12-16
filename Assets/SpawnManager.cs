using Assets.ServiceLocator;
using Assets.Structures;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Assets
{
    public class SpawnManager : LocatableMonoBehaviorBase
    {
        public delegate void SpawnCallback(GameObject spawnedObject);

        public override void Initialize()
        {
        }

        public void SpawnStructure(IStructureFacade structureFacade, Vector3 position, SpawnCallback callback)
        {
            SpawnAddressable(structureFacade.AssetName, position, transform, callback);
        }

        private void SpawnAddressable(string address, Vector3 position, Transform parent, SpawnCallback callback)
        {
            var op = Addressables.InstantiateAsync(address, position, Quaternion.identity, parent);
            op.Completed += (AsyncOperationHandle<GameObject> obj) => callback.Invoke(obj.Result);
        }

        public void SpawnUIElement(string name, Transform parent, SpawnCallback callback)
        {
            SpawnAddressable(name, parent.transform.position, parent, callback);
        }

        public void AddItemToDestroy(GameObject gameObject)
        {
            lock (_destroyCache)
            {
                _destroyCache.Add(gameObject);
            }
        }

        public void Update()
        {
            DestroyItemsInCache();
        }

        private readonly List<GameObject> _destroyCache = new List<GameObject>();

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
    }
}