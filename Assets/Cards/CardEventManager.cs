using Assets.Factions;
using Assets.Map;
using UnityEngine;

namespace Assets.Cards
{
    public static class CardDelegates
    {
        public delegate void CardDiscarded(ICard card);

        public delegate void CardPlayed(ICard card, Coord coord);

        public delegate void CardReceived(ICard card, IFaction player);

        public delegate void CardRotated(ICard card);

        public delegate void DeckRecyled(IDeck deck);

        public delegate void DeckShuffled(IDeck deck);

        public delegate void SetPlayerCardActive(ICard card);
    }

    public static class CardEventManager
    {
        public static event CardDelegates.CardDiscarded OnCardDiscarded;

        public static event CardDelegates.CardPlayed OnCardPlayed;

        public static event CardDelegates.CardPlayed OnCardPreviewed;

        public static event CardDelegates.CardReceived OnCardReceived;

        public static event CardDelegates.CardRotated OnCardRotated;
        public static event CardDelegates.DeckRecyled OnDeckRecyled;

        public static event CardDelegates.DeckShuffled OnDeckShuffled;

        public static event CardDelegates.SetPlayerCardActive OnSetPlayerCardActive;
        public static void CardDiscarded(ICard card)
        {
            Debug.Log($"Card discarded {card}");
            OnCardDiscarded?.Invoke(card);
        }

        public static void CardPlayed(ICard card, Coord coord)
        {
            Debug.Log($"Card played {card}");
            OnCardPlayed?.Invoke(card, coord);
        }

        public static void CardPreviewed(ICard card, Coord coord)
        {
            OnCardPreviewed?.Invoke(card, coord);
        }

        public static void CardReceived(ICard card, IFaction player)
        {
            Debug.Log($"Card dealt {card}");
            OnCardReceived?.Invoke(card, player);
        }

        public static void CardRotated(ICard card)
        {
            Debug.Log($"Card rotated {card}");
            OnCardRotated?.Invoke(card);
        }

        public static void DeckRecyled(IDeck deck)
        {
            Debug.Log($"Deck Recyled {deck}");
            OnDeckRecyled?.Invoke(deck);
        }

        public static void DeckShuffled(IDeck deck)
        {
            Debug.Log($"Deck Shuffled {deck}");
            OnDeckShuffled?.Invoke(deck);
        }
        public static void SetPlayerCardActive(ICard card)
        {
            Debug.Log($"Card active {card}");
            OnSetPlayerCardActive?.Invoke(card);
        }
    }
}