using Assets.Cards;
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

            CardEventManager.OnSetPlayerCardActive += OnPlayerCardActive;
            CardEventManager.OnRotateCardsCW += RotateCardCW;
            CardEventManager.OnRotateCardsCCW += RotateCardCCW;
        }

        private ICard _activeCard;

        private void OnPlayerCardActive(ICard card)
        {
            if (Hand.Contains(card))
            {
                _activeCard = card;
            }
            else
            {
                throw new System.Exception($"Card not in hand!: {card}");
            }
        }

        private void RotateCardCW()
        {
            foreach (var card in Hand)
            {
                card.RotateCW();
            }
        }

        private void RotateCardCCW()
        {
            foreach (var card in Hand)
            {
                card.RotateCCW();
            }
        }

        public ICard GetActiveCard()
        {
            if (_activeCard == null)
            {
                _activeCard = Hand[0];
            }
            return _activeCard;
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