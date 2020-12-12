using Assets.ServiceLocator;
using Assets.Structures;
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
            var op = Addressables.InstantiateAsync(structureFacade.AssetName, position, Quaternion.identity, transform);
            op.Completed += (AsyncOperationHandle<GameObject> obj) => callback.Invoke(obj.Result);
        }
    }
}