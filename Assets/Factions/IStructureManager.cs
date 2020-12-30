using Assets.Map;
using Assets.Resources;
using Assets.Structures;
using System.Collections.Generic;

namespace Assets.Factions
{
    public interface IStructureManager
    {
        void AddStructure(IStructureFacade selectedFacade, ICoord coord);

        void RemoveStructure(IStructure structure);

        List<IStructureFacade> GetBuildableStructures();

        List<IStructure> GetStructures();

        ICoord GetFactionCoreLocation();

        void DoTurnEndActions();

        void DoTurnStartActions();

        List<(ResourceType, int)> GetCombinedYield();
    }
}