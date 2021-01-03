using UnityEngine;

namespace Assets.UI
{
    public class RadialMenuManager
    {
        private ISpawnManager _spawnManager;
        private Transform _parent;

        public RadialMenuManager(ISpawnManager spawnManager, Transform parent)
        {
            _spawnManager = spawnManager;
            _parent = parent;
        }

        public void ShowRadialMenu(bool closeOnSelect, params (string label, RadialMenuDelegates.MenuItemClicked onClick)[] options)
        {
            _spawnManager.SpawnUIElement("RadialMenu", _parent, (radialmenuObj) =>
            {
                var menu = radialmenuObj.GetComponent<RadialMenuController>();
                menu.CloseOnSelect = closeOnSelect;
                foreach (var (label, onClick) in options)
                {
                    menu.AddButton(label, onClick);
                }
            }
            );
        }
    }
}