using Assets.Structures;
using UnityEngine;

namespace Assets
{
    public interface ISpawnManager
    {
        void AddItemToDestroy(GameObject gameObject);
        void DestroyItemsInCache();
        void SpawnStructure(IStructureFacade structureFacade, Vector3 position, SpawnManager.SpawnCallback callback);
        void SpawnUIElement(string name, Transform parent, SpawnManager.SpawnCallback callback);
    }
}