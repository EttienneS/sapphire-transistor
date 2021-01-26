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
        private readonly IFactionManager _factionManager;
        private readonly IUIManager _uiManager;

        public PlayerFaction(string name, IServiceLocator serviceLocator) : base(name, serviceLocator)
        {
            _uiManager = serviceLocator.Find<IUIManager>();
            _factionManager = serviceLocator.Find<IFactionManager>();
            CellEventManager.OnCellClicked += CellClicked;
        }

        public void CellClicked(Cell cell)
        {
            if (UIHelper.MouseOverUi())
            {
                return;
            }

            if (_factionManager.TryGetStructureInCell(cell, out IStructure structure))
            {
                var activeCard = _cards[0];
                if (structure.Type == StructureType.Anchor && activeCard.CanPlay(cell.Coord))
                {
                    PlayCard(activeCard, cell.Coord);
                }
                else
                {
                    ShowStructureInfo(structure);
                }
            }
            else
            {
                _uiManager.MessageManager.HideAll();
            }
        }

        public void ResetUI()
        {
            _uiManager.DisableHighlights();
            _uiManager.MessageManager.HideAll();
        }

        public override void TakeTurn()
        {
        }

        private void ShowStructureInfo(IStructure structure)
        {
            Debug.Log($"{structure.Type}: {structure.OccupiedCoords[0]}");
            _uiManager.MessageManager.ShowMessage(structure.Type.ToString(), structure.GetStatus());

            //var radialMenuOptions = new List<RadialMenuOptionFacade>();
            //if (StructureManager.GetStructures().Contains(structure))
            //{
            //    radialMenuOptions.Add(new RadialMenuOptionFacade($"Remove {structure.Type}", () => { },
            //                                                                                 () => StructureManager.RemoveStructure(structure)));
            //}
            //_uiManager.RadialMenuManager.ShowRadialMenu(closeOnSelect: true,
            //                                            onMenuClose: () => ResetUI(),
            //                                            radialMenuOptions);
        }
    }
}