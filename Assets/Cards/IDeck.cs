using Assets.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Cards
{
    public interface IDeck
    {
        void AddCard(ICard card);

        void AddToDiscardPile(ICard card);

        ICard Draw();

        void Shuffle();
    }
}