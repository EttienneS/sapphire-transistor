﻿using Assets.Cards;
using Assets.Helpers;
using Assets.Map;
using Assets.ServiceLocator;
using Assets.Structures;
using Assets.UI;
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
            InputEventManager.OnRotateCardCW += RotateCardCW;
            InputEventManager.OnRotateCardCCW += RotateCardCCW;
        }

        private void RotateCardCW()
        {
            var card = GetActiveCard();
            card.RotateCW();
            Debug.Log(card.ToString());
        }

        private void RotateCardCCW()
        {
            var card = GetActiveCard();
            card.RotateCCW();
            Debug.Log(card.ToString());
        }

        public ICard GetActiveCard()
        {
            return Hand[0];
        }

        public void CellClicked(Cell cell)
        {
            if (UIHelper.MouseOverUi())
            {
                return;
            }

            if (_factionManager.TryGetStructureInCell(cell, out IStructure structure))
            {
                var activeCard = GetActiveCard();

                Debug.Log(activeCard.ToString());

                var anchor = activeCard.GetRelativeAnchorPoint(cell.Coord);

                if (structure.Type == StructureType.Anchor && activeCard.CanPlay(anchor))
                {
                    PlayCard(activeCard, anchor);
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