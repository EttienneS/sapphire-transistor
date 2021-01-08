using Assets.Helpers;
using Assets.Map;
using Assets.ServiceLocator;
using Assets.Structures;
using Assets.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Factions
{
    public class PlayerFaction : FactionBase
    {
        private IUIManager _uiManager;
        private IFactionManager _factionManager;

        public PlayerFaction(string name, IServiceLocator serviceLocator) : base(name, serviceLocator)
        {
            _uiManager = serviceLocator.Find<IUIManager>();
            _factionManager = serviceLocator.Find<IFactionManager>();
            CellEventManager.OnCellClicked += CellClicked;
        }

        public override void TakeTurn()
        {
        }

        public void CellClicked(Cell cell)
        {
            if (UIHelper.MouseOverUi())
            {
                return;
            }

            if (_factionManager.TryGetStructureInCell(cell, out IStructure structure))
            {
                ShowStructureInfo(structure);
            }
            else
            {
                ShowBuildRadialMenu(cell);
            }
        }

        private void ShowStructureInfo(IStructure structure)
        {
            _uiManager.HighlightCell(structure.Coord, Color.blue);
            Debug.Log($"{structure.Name}: {structure.Coord}");
        }

        private void ShowBuildRadialMenu(Cell cell)
        {
            var radialMenuOptions = new List<(string, RadialMenuDelegates.MenuItemClicked)>();

            foreach (var structure in StructureManager.GetBuildableStructures())
            {
                radialMenuOptions.Add(($"{structure.Name}", () =>
                {
                    StructureManager.AddStructure(structure, cell.Coord);
                }
                ));
            }

            _uiManager.HighlightCell(cell.Coord, Color.red);
            _uiManager.RadialMenuManager.ShowRadialMenu(closeOnSelect: true,
                                                        onMenuClose: () => _uiManager.DisableHighlight(),
                                                        radialMenuOptions);
        }
    }
}