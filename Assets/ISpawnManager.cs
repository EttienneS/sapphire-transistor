using Assets.Structures;
using UnityEngine;

namespace Assets
{
    public interface ISpawnManager
    {
        void AddItemToDestroy(GameObject gameObject);
        void DestroyItemsInCache();
        void SpawnModel(string meshName, Vector3 position, SpawnManager.SpawnCallback callback);
        void SpawnUIElement(string name, Transform parent, SpawnManager.SpawnCallback callback);
        void RecyleItem(string pool, GameObject gameObject);
    }
}