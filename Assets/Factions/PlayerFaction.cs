using Assets.Helpers;
using Assets.Map;
using Assets.ServiceLocator;
using Assets.Structures;
using Assets.UI;
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
            Debug.Log($"{structure.Name}: {structure.Coord}");
            var radialMenuOptions = new List<RadialMenuOptionFacade>
            {
                new RadialMenuOptionFacade($"Remove {structure.Name}", () => StructureManager.RemoveStructure(structure))
            };

            HighlightAndShowRadialMenu(structure.Coord, Color.blue, radialMenuOptions);
        }

        private void HighlightAndShowRadialMenu(ICoord coord, Color color, List<RadialMenuOptionFacade> radialMenuOptions)
        {
            _uiManager.HighlightCell(coord, color);
            _uiManager.RadialMenuManager.ShowRadialMenu(closeOnSelect: true,
                                                        onMenuClose: () => _uiManager.DisableHighlight(),
                                                        radialMenuOptions);
        }

        private void ShowBuildRadialMenu(Cell cell)
        {
            var radialMenuOptions = new List<RadialMenuOptionFacade>();

            foreach (var structure in StructureManager.GetBuildableStructures())
            {
                var placementCheck = structure.CanBePlacedInCell(cell);
                if (placementCheck.CanPlace)
                {
                    radialMenuOptions.Add(new RadialMenuOptionFacade($"{structure.Name}", () => StructureManager.AddStructure(structure, cell.Coord)));
                }
                else
                {
                    radialMenuOptions.Add(new RadialMenuOptionFacade($"Can't place: {structure.Name} {placementCheck.Message}", () => { }, false));
                }
            }

            HighlightAndShowRadialMenu(cell.Coord, Color.red, radialMenuOptions);
        }
    }
}