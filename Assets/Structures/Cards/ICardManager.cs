using Assets.Factions;

namespace Assets.Structures.Cards
{
    public interface ICardManager
    {
        ICard GetRandomCard();

        void DealCards(IFaction faction);
    }
}