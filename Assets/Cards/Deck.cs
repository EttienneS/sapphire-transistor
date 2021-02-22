using Assets.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Cards
{
    public class Deck : IDeck
    {
        private readonly Queue<ICard> _cards;
        private readonly List<ICard> _discardPile;

        public string Name { get; }

        public Deck(string name)
        {
            Name = name;
            _cards = new Queue<ICard>();
            _discardPile = new List<ICard>();
        }

        public override string ToString()
        {
            return Name;
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
                RecyleDeck();
                Shuffle();
            }

            return _cards.Dequeue();
        }

        public void RecyleDeck()
        {
            foreach (var card in _discardPile)
            {
                AddCard(card);
            }
            _discardPile.Clear();

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
    }
}