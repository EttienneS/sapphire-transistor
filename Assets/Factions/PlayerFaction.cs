﻿using Assets.Helpers;
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
            _uiManager.MessageManager.ShowMessage(structure.Name, structure.GetStatus());
            _uiManager.RadialMenuManager.ShowRadialMenu(closeOnSelect: true,
                                                        onMenuClose: () => ResetUI(),
                                                        radialMenuOptions);
        }

        public void ResetUI()
        {
            _uiManager.DisableHighlights();
            _uiManager.MessageManager.HideAll();
        }

        //private void PlaceFacadeIfPossible(Cell cell, IStructureFacade facade)
        //{
        //    if (facade.CheckCellPlacement(cell).CanPlace && CanAfford(facade.Cost))
        //    {
        //        StructureManager.AddStructure(facade, cell.Coord);
        //        foreach (var cost in facade.Cost)
        //        {
        //            ModifyResource(cost.Item1, -cost.Item2);
        //        }
        //    }
        //}

        //private void ShowFacadeFootprintOutline(Cell cell, IStructureFacade structure)
        //{
        //    var placementCheck = structure.CheckCellPlacement(cell);
        //    var color = placementCheck.CanPlace ? Color.green : Color.red;
        //    _uiManager.HighlightCells(structure.GetPlacementCoords(cell.Coord), color);
        //}
    }
}