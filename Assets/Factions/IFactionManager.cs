namespace Assets.Factions
{
    public interface IFactionManager
    {
        void AddFaction(IFaction faction);

        IFaction GetActiveFaction();

        void MoveToNextTurn();

        IFaction GetPlayerFaction();

    }
}