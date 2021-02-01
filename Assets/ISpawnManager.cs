using Assets.Structures;
using UnityEngine;

namespace Assets
{
    public interface ISpawnManager
    {
        void AddItemToDestroy(GameObject gameObject);

        void DestroyItemsInCache();

        void SpawnModel(string meshName, Vector3 position, SpawnManager.SpawnCallback callback);
        void SpawnModel(StructureType type, Vector3 position, SpawnManager.SpawnCallback callback);

        void SpawnPreviewModel(string address, Vector3 position, bool valid, SpawnManager.SpawnCallback callback);
        void SpawnPreviewModel(StructureType type, Vector3 position, bool valid, SpawnManager.SpawnCallback callback);

        void SpawnUIElement(string name, Transform parent, SpawnManager.SpawnCallback callback);

        void RecyleItem(string pool, GameObject gameObject);
        void RecyleItem(StructureType type, GameObject gameObject);
    }
}