using Assets.Resources;

namespace Assets.Factions
{
    public static class FactionDelegates
    {
        public delegate void OnTurnEnded(IFaction faction);

        public delegate void OnResourceChanged(ResourceType resourceType, int newValue);
    }
}