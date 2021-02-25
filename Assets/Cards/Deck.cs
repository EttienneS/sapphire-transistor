using Assets.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Cards
{
    public class Deck : IDeck
    {
        private readonly Queue<ICard> _cards;
        private readonly List<ICard> _discardPile;

        public CardColor Color { get; }

        public Deck(CardColor color)
        {
            Color = color;
            _cards = new Queue<ICard>();
            _discardPile = new List<ICard>();

            CardEventManager.OnCardDiscarded += OnCardDiscarded;
        }

        private void OnCardDiscarded(ICard card)
        {
            if (card.Color == Color)
            {
                AddToDiscardPile(card);
            }
        }

        public override string ToString()
        {
            return Color.ToString();
        }

        public void AddCard(ICard card)
        {
            _cards.Enqueue(card);
        }

        public void AddToDiscardPile(ICard card)
        {
            _discardPile.Add(card);
        }

        public ICard Draw()
        {
            if (_cards.Count == 0)
            {
                throw new System.Exception("No cards to draw!");
            }

            return _cards.Dequeue();
        }

        public void Recyle()
        {
            foreach (var card in _discardPile)
            {
                AddCard(card);
            }
            _discardPile.Clear();
            Shuffle();
            CardEventManager.DeckRecyled(this);
        }

        public void Shuffle()
        {
            var shuffled = _cards.ToList();
            shuffled.Shuffle();
            _cards.Clear();
            foreach (var card in shuffled)
            {
                _cards.Enqueue(card);
            }

            CardEventManager.DeckShuffled(this);
        }

        public int GetRemaining()
        {
            return _cards.Count;
        }

        public int GetDiscardPile()
        {
            return _discardPile.Count;
        }
    }
}