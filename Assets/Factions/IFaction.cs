using Assets.Map;
using Assets.Resources;
using System.Collections.Generic;

namespace Assets.Factions
{
    public interface IFaction
    {
        event FactionDelegates.OnResourceChanged OnResourcesUpdated;

        event FactionDelegates.OnTurnEnded TurnEnded;

        event FactionDelegates.OnTurnStarted TurnStarted;

        string Name { get; }

        void DoTurnEndActions();

        void DoTurnStartActions();

        Dictionary<ResourceType, int> GetResources();

        IStructureManager StructureManager { get; }

        void ModifyResource(ResourceType resource, int amount);

        void TakeTurn();
    }
}