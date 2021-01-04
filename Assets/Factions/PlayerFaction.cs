using Assets.Helpers;
using Assets.Map;
using Assets.ServiceLocator;
using Assets.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Factions
{
    public class PlayerFaction : FactionBase
    {
        private IUIManager _uiManager;

        public PlayerFaction(string name, IServiceLocator serviceLocator) : base(name, serviceLocator)
        {
            _uiManager = serviceLocator.Find<IUIManager>();
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

            var radialMenuOptions = new List<(string, RadialMenuDelegates.MenuItemClicked)>();

            foreach (var structure in StructureManager.GetBuildableStructures())
            {
                radialMenuOptions.Add(($"{structure.Name}", () =>
                {
                    StructureManager.AddStructure(structure, cell.Coord);
                }
                ));
            }

            _uiManager.HighlightCell(cell, Color.red);
            _uiManager.RadialMenuManager.ShowRadialMenu(closeOnSelect: true,
                                                        onMenuClose: () => _uiManager.DisableHighlight(),
                                                        radialMenuOptions);
        }
    }
}