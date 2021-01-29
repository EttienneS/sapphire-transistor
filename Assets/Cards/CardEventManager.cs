﻿using Assets.Factions;
using Assets.Map;
using System;
using UnityEngine;

namespace Assets.Cards
{
    public static class CardDelegates
    {
        public delegate void SetPlayerCardActive(ICard card);

        public delegate void CardPlayed(ICard card, IFaction player, ICoord coord);

        public delegate void CardReceived(ICard card, IFaction player);
    }

    public static class CardEventManager
    {
        public static event CardDelegates.SetPlayerCardActive OnSetPlayerCardActive;

        public static event CardDelegates.CardPlayed OnCardPlayed;

        public static event CardDelegates.CardReceived OnCardReceived;

        public static void CardPlayed(ICard card, IFaction player, ICoord coord)
        {
            Debug.Log($"Card played {card}");
            OnCardPlayed?.Invoke(card, player, coord);
        }

        public static void SetPlayerCardActive(ICard card)
        {
            Debug.Log($"Card active {card}");
            OnSetPlayerCardActive?.Invoke(card);
        }

        public static void CardReceived(ICard card, IFaction player)
        {
            Debug.Log($"Card dealt {card}");
            OnCardReceived?.Invoke(card, player);
        }
    }
}