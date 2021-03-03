using Assets.Factions;
using Assets.Map;
using System.Collections.Generic;

namespace Assets.Structures
{
    public interface IStructure
    {
        IStructureBehaviour Behaviour { get; }
        Coord Coord { get; }
        bool RequiresLink { get; }
        StructureDefinition.StructureType Type { get; }

        string GetStatus();

        Dictionary<ResourceType, int> GetYield(IStructure structure);

        void TurnEnd(IStructure structure);

        void TurnStart(IStructure structure);
    }
}