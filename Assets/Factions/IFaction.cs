using Assets.Actors;
using Assets.Map;
using Assets.Resources;
using Assets.Structures;
using System.Collections.Generic;

namespace Assets.Factions
{
    public interface IFaction
    {
        string Name { get; }

        List<IStructureFacade> GetBuildableStructures();

        void ModifyResource(ResourceType resource, int amount);

        Dictionary<ResourceType, int> GetResources();

        IActor GetFactionHead();

        List<IActor> GetActors();
        void AddActor(IActor actor);

        void SetFactionHead(IActor actor);

        List<IStructure> GetStructures();

        void DoTurnStartActions();

        void TakeTurn();

        void DoTurnEndActions();

        void AddStructure(IStructureFacade selectedFacade, ICoord coord);

        event FactionDelegates.OnResourceChanged OnResourcesUpdated;

        event FactionDelegates.OnTurnEnded TurnEnded;
    }
}