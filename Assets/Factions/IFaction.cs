using Assets.Actors;
using Assets.Map;
using Assets.Resources;
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

        void AddActor(IActor actor);

        void AddStructure(IStructureFacade selectedFacade, ICoord coord);

        void DoTurnEndActions();

        void DoTurnStartActions();

        List<IActor> GetActors();

        List<IStructureFacade> GetBuildableStructures();

        IActor GetFactionHead();

        Dictionary<ResourceType, int> GetResources();

        List<IStructure> GetStructures();

        void ModifyResource(ResourceType resource, int amount);
        void SetFactionHead(IActor actor);
        void TakeTurn();
    }
}