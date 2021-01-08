using System.Collections.Generic;
using UnityEngine;

namespace Assets.UI
{
    public class RadialMenuManager
    {
        private readonly ISpawnManager _spawnManager;
        private readonly Transform _parent;
        private RadialMenuController _activeMenu;

        public RadialMenuManager(ISpawnManager spawnManager, Transform parent)
        {
            _spawnManager = spawnManager;
            _parent = parent;
        }

        public void ShowRadialMenu(bool closeOnSelect, RadialMenuDelegates.MenuClosed onMenuClose, IEnumerable<(string label, RadialMenuDelegates.MenuItemClicked onClick)> options)
        {
            if (_activeMenu != null)
            {
                _activeMenu.CloseMenu();
            }

            _spawnManager.SpawnUIElement("RadialMenu", _parent, (radialmenuObj) =>
            {
                _activeMenu = radialmenuObj.GetComponent<RadialMenuController>();
                _activeMenu.CloseOnSelect = closeOnSelect;
                foreach (var (label, onClick) in options)
                {
                    _activeMenu.AddButton(label, onClick);
                }

                _activeMenu.AddButton("Cancel", () => _activeMenu.CloseMenu());
                _activeMenu.MenuClosed += onMenuClose;
            }
            );
        }
    }
}