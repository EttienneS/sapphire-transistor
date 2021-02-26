using Assets.Factions;
using Assets.Map;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Cards
{
    public class HandManager : IHandManager
    {
        private ICard _activeCard;
        private (ICard card, ICoord coord)? _activePreview;
        private IFaction _owner;

        public HandManager(IFaction owner)
        {
            _owner = owner;

            Hand = new List<ICard>();

            CardEventManager.OnSetPlayerCardActive += OnPlayerCardActive;
            CardEventManager.OnCardRotated += OnCardRotated;
        }

        private void OnCardRotated(ICard card)
        {
            if (card == _activePreview?.card)
            {
                ClearPreview();
            }
        }

        public List<ICard> Hand { get; }

        public void CancelCard()
        {
            ClearPreview();
        }

        public void CellClicked(Cell cell)
        {
            if (TryGetActiveCard(out ICard activeCard) && PreviewValid(cell, activeCard))
            {
                ConfirmCard();
            }
        }

        public void CellHover(Cell cell)
        {
            if (TryGetActiveCard(out ICard activeCard))
            {
                if (_activePreview?.card != activeCard || _activePreview?.coord != cell.Coord)
                {
                    PreviewCard(activeCard, cell.Coord);
                }
            }
        }

        public void ClearPreview()
        {
            if (_activePreview.HasValue)
            {
                _activePreview.Value.card.ClearPreview();
                _activePreview = null;
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
            CardEventManager.CardDiscarded(card);
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

        public void DrawCard(ICard card)
        {
            if (Hand.Count >= GetMaxHandSize())
            {
                throw new System.Exception("Too many cards in hand!");
            }

            Hand.Add(card);
            CardEventManager.CardReceived(card, _owner);
        }

        public int GetMaxHandSize()
        {
            return 5;
        }

        public int GetOpenHandSize()
        {
            return GetMaxHandSize() - Hand.Count;
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

        private bool PreviewValid(Cell cell, ICard activeCard)
        {
            return _activePreview.HasValue
                && _activePreview.Value.coord == cell.Coord
                && _activePreview.Value.card == activeCard;
        }
    }
}