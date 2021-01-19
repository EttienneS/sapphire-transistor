using UnityEngine;

namespace Assets.UI
{
    public class MessageManager
    {
        private readonly ISpawnManager _spawnManager;
        private readonly Transform _parent;
        private MessagePanelController _currentMessage;

        public MessageManager(ISpawnManager spawnManager, Transform parent)
        {
            _spawnManager = spawnManager;
            _parent = parent;
        }

        public void ShowMessage(string title, string message)
        {
            HideAll();

            _spawnManager.SpawnUIElement("MessagePanel", _parent, (obj) =>
            {
                _currentMessage = obj.GetComponent<MessagePanelController>();
                _currentMessage.transform.position += new Vector3(0, 400f);
                _currentMessage.Show(title, message);
            });
        }

        internal void HideAll()
        {
            if (_currentMessage != null)
            {
                _currentMessage.Close();
                _currentMessage = null;
            }
        }
    }
}