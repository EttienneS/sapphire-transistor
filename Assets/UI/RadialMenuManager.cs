﻿using System.Collections.Generic;
using UnityEngine;

namespace Assets.UI
{
    public class RadialMenuOptionFacade
    {
        public string Text { get; }
        public RadialMenuDelegates.MenuItemClicked OnClick { get; }
        public bool Enabled { get; }
        public RadialMenuOptionFacade(string text, RadialMenuDelegates.MenuItemClicked onClick, bool enabled = true)
        {
            Text = text;
            OnClick = onClick;
            Enabled = enabled;
        }
    }

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

        public void ShowRadialMenu(bool closeOnSelect, RadialMenuDelegates.MenuClosed onMenuClose, IEnumerable<RadialMenuOptionFacade> options)
        {
            if (_activeMenu != null)
            {
                _activeMenu.CloseMenu();
            }

            _spawnManager.SpawnUIElement("RadialMenu", _parent, (radialmenuObj) =>
            {
                _activeMenu = radialmenuObj.GetComponent<RadialMenuController>();
                _activeMenu.CloseOnSelect = closeOnSelect;
                foreach (var option in options)
                {
                    _activeMenu.AddButton(option.Text, option.OnClick, option.Enabled);
                }

                _activeMenu.AddButton("Cancel", () => _activeMenu.CloseMenu());
                _activeMenu.MenuClosed += onMenuClose;
                _activeMenu.SetDefaults();
            }
            );
        }
    }
}