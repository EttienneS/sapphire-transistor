using System.Collections.Generic;
using UnityEngine;

namespace Assets.UI
{
    public class RadialMenuManager
    {
        private readonly ISpawnManager _spawnManager;
        private readonly Transform _parent;

        public RadialMenuManager(ISpawnManager spawnManager, Transform parent)
        {
            _spawnManager = spawnManager;
            _parent = parent;
        }

        public void ShowRadialMenu(bool closeOnSelect, RadialMenuDelegates.MenuClosed onMenuClose, IEnumerable<(string label, RadialMenuDelegates.MenuItemClicked onClick)> options)
        {
            _spawnManager.SpawnUIElement("RadialMenu", _parent, (radialmenuObj) =>
            {
                var menu = radialmenuObj.GetComponent<RadialMenuController>();
                menu.CloseOnSelect = closeOnSelect;
                foreach (var (label, onClick) in options)
                {
                    menu.AddButton(label, onClick);
                }

                menu.AddButton("Cancel", () => menu.CloseMenu());
                menu.MenuClosed += onMenuClose;
            }
            );
        }
    }
}