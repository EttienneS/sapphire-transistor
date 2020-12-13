namespace Assets.Factions
{
    public interface IFactionManager
    {
        void AddFaction(IFaction faction);

        IFaction GetActiveFaction();

        void MoveToNextTurn();

        IFaction GetPlayerFaction();

        event FactionDelegates.OnTurnEnded OnTurnEnded;
        event FactionDelegates.OnTurnStarted OnTurnStarted;

    }
}