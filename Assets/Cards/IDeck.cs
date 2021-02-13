using Assets.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Cards
{
    public interface IDeck
    {
        void AddCard(ICard card);

        ICard Draw();

        void Shuffle();
    }

    public class Deck : IDeck
    {
        private readonly Queue<ICard> _cards;

        public Deck()
        {
            _cards = new Queue<ICard>();
        }

        public void AddCard(ICard card)
        {
            _cards.Enqueue(card);
        }



        public ICard Draw()
        {
            return _cards.Dequeue();
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
        }
    }
}