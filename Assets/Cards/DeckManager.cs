using Assets.Factions;
using Assets.Map;
using Assets.ServiceLocator;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Cards
{
    public class DeckManager : IDeckManager
    {
        private ICard _activeCard;
        private (ICard card, ICoord coord)? _activePreview;
        private IFaction _owner;

        public DeckManager(IFaction owner)
        {
            _owner = owner;

            _deck = new Deck("Standard");
            Hand = new List<ICard>();

            DealCards();

            CardEventManager.OnSetPlayerCardActive += OnPlayerCardActive;
        }

        private void DealCards()
        {
            var cardMan = Locator.Instance.Find<ICardManager>();
            for (int i = 0; i < 25; i++)
            {
                _deck.AddCard(cardMan.GetRandomCard(_owner));
            }
        }


        private IDeck _deck;

        public List<ICard> Hand { get; }

        public void CancelCard()
        {
            ClearPreview();
        }

        public void CellClicked(Cell cell)
        {
            if (TryGetActiveCard(out ICard activeCard))
            {
                if (_activePreview.HasValue && _activePreview.Value.coord == cell.Coord && _activePreview.Value.card == activeCard)
                {
                    ConfirmCard();
                }
                else
                {
                    PreviewCard(activeCard, cell.Coord);
                }
            }
        }

        public void ConfirmCard()
        {
            var card = _activePreview.Value.card;
            var coord = _activePreview.Value.coord;
            var cost = card.GetCost();

            if (card.CanPlay(coord) && _owner.CanAfford(cost))
            {
                card.Play(coord);
                _owner.ModifyResource(cost);

                DiscardCard(_activeCard);
                _activeCard = null;
            }
            ClearPreview();
        }

        public void DiscardCard(ICard card)
        {
            Hand.Remove(card);
            _deck.AddToDiscardPile(card);
            CardEventManager.CardDiscarded(card);
        }

        public void DrawCard()
        {
            var card = _deck.Draw();
            Hand.Add(card);
            CardEventManager.CardReceived(card, _owner);
        }

        public void DrawToHandSize()
        {
            var cardsToDraw = GetMaxHandSize() - Hand.Count;

            if (cardsToDraw > 0)
            {
                for (int i = 0; i < cardsToDraw; i++)
                {
                    DrawCard();
                }
            }
        }

        public int GetMaxHandSize()
        {
            return 5;
        }

        public void OnPlayerCardActive(ICard card)
        {
            if (Hand.Contains(card))
            {
                ClearPreview();
                _activeCard = card;
            }
            else
            {
                throw new System.Exception($"Card not in hand!: {card}");
            }
        }

        public void PreviewCard(ICard card, ICoord coord)
        {
            ClearPreview();
            _activePreview = (card, coord);
            _activePreview.Value.card.Preview(_activePreview.Value.coord);
        }

        public bool TryGetActiveCard(out ICard card)
        {
            card = _activeCard;
            if (_activeCard == null)
            {
                return false;
            }
            return true;
        }

        public void ClearPreview()
        {
            if (_activePreview.HasValue)
            {
                _activePreview.Value.card.ClearPreview();
                _activePreview = null;
            }
        }

        public void DiscardHand()
        {
            foreach (var card in Hand.ToList())
            {
                DiscardCard(card);
            }
            _activeCard = null;
            ClearPreview();
        }
    }
}