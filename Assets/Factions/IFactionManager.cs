using Assets.ServiceLocator;

namespace Assets.Factions
{
    public interface IFactionManager
    {
        void AddFaction(IFaction faction);

        IFaction GetActiveFaction();

        void MoveToNextTurn();
    }
}