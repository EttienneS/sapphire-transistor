using Assets.Cards;
using Assets.Helpers;
using Assets.Map;
using Assets.ServiceLocator;
using Assets.Structures;
using Assets.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Factions
{
    public class PlayerFaction : FactionBase
    {
        public IHandManager HandManager;
        private readonly CardLoader _cardLoader;
        private readonly IUIManager _uiManager;

        public Dictionary<CardColor, IDeck> Decks { get; set; }

        public PlayerFaction(string name, IServiceLocator serviceLocator) : base(name, serviceLocator)
        {
            _uiManager = serviceLocator.Find<IUIManager>();

            CellEventManager.OnCellClicked += CellClicked;
            CellEventManager.OnMouseOver += CellHover;

            _cardLoader = new CardLoader(this);
            HandManager = new HandManager(this);

            Decks = new Dictionary<CardColor, IDeck>();
            foreach (var card in _cardLoader.GetAvailableCards())
            {
                if (!Decks.ContainsKey(card.Color))
                {
                    Decks.Add(card.Color, new Deck(card.Color));
                }
                
                Decks[card.Color].AddCard(card);
            }
        }

        public void CellClicked(Cell cell)
        {
            if (UIHelper.MouseOverUi())
            {
                return;
            }

            HandManager.CellClicked(cell);
        }

        public void CellHover(Cell cell)
        {
            if (UIHelper.MouseOverUi())
            {
                return;
            }

            HandManager.CellHover(cell);
        }

        public override void EndTurn()
        {
            Locator.Instance.Find<IUIManager>().HideDrawView();

            HandManager.DiscardHand();
            base.EndTurn();
        }

        public void ResetUI()
        {
            _uiManager.DisableHighlights();
            _uiManager.MessageManager.HideAll();
        }

        public override void TakeTurn()
        {
            var connected = StructureManager.GetStructuresLinkedTo(StructureManager.GetCore());

            ReadyDecks();

            Locator.Instance.Find<IUIManager>().ShowDrawView();

            HighlightConnected(connected);
            GetYieldForConnected(connected);
        }

        private void ReadyDecks()
        {
            foreach (var deck in Decks)
            {
                if (deck.Value.GetRemaining() <= 0)
                {
                    deck.Value.Recyle();
                }
            }
        }

        private void GetYieldForConnected(System.Collections.Generic.List<IStructure> connected)
        {
            foreach (var structure in connected)
            {
                foreach (var resource in structure.GetYield(structure))
                {
                    ModifyResource(resource.Key, resource.Value);
                }
            }
        }

        private void HighlightConnected(System.Collections.Generic.List<IStructure> connected)
        {
            foreach (var structure in connected)
            {
                StructureEventManager.HideHighlight(structure);
            }

            foreach (var structure in StructureManager.GetStructures().Except(connected).Where(s => s.RequiresLink))
            {
                StructureEventManager.ShowHiglight(structure);
            }
        }
    }
}