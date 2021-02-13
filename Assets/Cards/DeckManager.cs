using Assets.Factions;
using Assets.Map;
using System.Collections.Generic;

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

            CardLoader = new CardLoader(_owner);
            Deck = new Deck();
            Hand = new List<ICard>();

            CardEventManager.OnSetPlayerCardActive += OnPlayerCardActive;
        }

        public ICardLoader CardLoader { get; }

        public IDeck Deck { get; }

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

                Hand.Remove(_activeCard);
                _activeCard = null;
            }
            ClearPreview();
        }

        public void DrawCard(ICard card)
        {
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
                    DrawCard(Deck.Draw());
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
    }
}