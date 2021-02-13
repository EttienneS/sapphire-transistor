using Assets.Structures;
using System.Collections.Generic;

namespace Assets.Factions
{
    public interface IFaction
    {
        event FactionDelegates.OnResourceChanged OnResourcesUpdated;

        event FactionDelegates.OnTurnEnded TurnEnded;

        event FactionDelegates.OnTurnStarted TurnStarted;

        string Name { get; }

        IStructureManager StructureManager { get; }

        bool CanAfford(Dictionary<ResourceType, int> cost);

        void DoTurnEndActions();

        void DoTurnStartActions();

        Dictionary<ResourceType, int> GetResources();

        void ModifyResource(ResourceType resource, int amount);

        void ModifyResource(Dictionary<ResourceType, int> cost);

        void TakeTurn();
    }
}