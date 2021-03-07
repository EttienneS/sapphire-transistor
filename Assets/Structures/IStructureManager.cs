using Assets.Map;
using System.Collections.Generic;

namespace Assets.Structures
{
    public interface IStructureManager
    {
        void AddStructure(StructureDefinition.StructureType type, Coord coord);

        void RemoveStructure(IStructure structure);

        List<IStructure> GetStructures();

        void DoTurnEndActions();

        void DoTurnStartActions();

        List<IStructure> GetStructuresLinkedTo(IStructure structure);
    }
}