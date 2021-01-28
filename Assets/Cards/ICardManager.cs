﻿using Assets.Factions;

namespace Assets.Cards
{
    public interface ICardManager
    {
        ICard GetRandomCard();

        void DealCards(IFaction faction);
    }
}