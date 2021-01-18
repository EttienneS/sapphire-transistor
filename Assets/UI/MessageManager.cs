using UnityEngine;

namespace Assets.UI
{
    public class MessageManager
    {
        private readonly ISpawnManager _spawnManager;
        private readonly Transform _parent;

        public MessageManager(ISpawnManager spawnManager, Transform parent)
        {
            _spawnManager = spawnManager;
            _parent = parent;
        }

        public void ShowMessage(string message)
        {
            Debug.Log(message);
        }
    }
}