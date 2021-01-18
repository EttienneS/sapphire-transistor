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
        private readonly IUIManager _uiManager;
        private readonly IFactionManager _factionManager;

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
            Debug.Log($"{structure.Name}: {structure.OccupiedCoords[0]}");

            var radialMenuOptions = new List<RadialMenuOptionFacade>();

            if (StructureManager.GetStructures().Contains(structure))
            {
                radialMenuOptions.Add(new RadialMenuOptionFacade($"Remove {structure.Name}", () => { },
                                                                                             () => StructureManager.RemoveStructure(structure)));
            }
            _uiManager.MessageManager.ShowMessage(structure.GetStatus());
            _uiManager.RadialMenuManager.ShowRadialMenu(closeOnSelect: true,
                                                        onMenuClose: () => _uiManager.DisableHighlights(),
                                                        radialMenuOptions);
        }

        private void ShowBuildRadialMenu(Cell cell)
        {
            var radialMenuOptions = new List<RadialMenuOptionFacade>();

            foreach (var facade in StructureManager.GetBuildableStructures())
            {
                radialMenuOptions.Add(new RadialMenuOptionFacade($"{facade.Name}",
                    onClick: () => ShowFacadeFootprintOutline(cell, facade),
                    onConfirm: () => PlaceFacadeIfPossible(cell, facade)));
            }

            _uiManager.RadialMenuManager.ShowRadialMenu(closeOnSelect: true,
                                                        onMenuClose: () => _uiManager.DisableHighlights(),
                                                        radialMenuOptions);
        }

        private void PlaceFacadeIfPossible(Cell cell, IStructureFacade facade)
        {
            if (facade.CheckCellPlacement(cell).CanPlace)
            {
                StructureManager.AddStructure(facade, cell.Coord);
            }
        }

        private void ShowFacadeFootprintOutline(Cell cell, IStructureFacade structure)
        {
            var placementCheck = structure.CheckCellPlacement(cell);
            var color = placementCheck.CanPlace ? Color.green : Color.red;
            _uiManager.HighlightCells(structure.GetPlacementCoords(cell.Coord), color);
        }
    }
}