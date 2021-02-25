using Assets.Map;
using System.Collections.Generic;

namespace Assets.Structures
{
    public interface IStructureManager
    {
        void AddStructure(StructureType type, ICoord coord);

        void RemoveStructure(IStructure structure);

        List<IStructure> GetStructures();

        void DoTurnEndActions();

        void DoTurnStartActions();

        IPlacementValidator PlacementValidator { get; }

        List<IStructure> GetStructuresLinkedTo(IStructure structure);
        IStructure GetCore();
    }
}