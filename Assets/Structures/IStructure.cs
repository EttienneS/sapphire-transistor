using Assets.Factions;
using Assets.Map;
using System.Collections.Generic;

namespace Assets.Structures
{
    public interface IStructure
    {
        StructureDefinition.StructureType Type { get; }

        IStructureBehaviour Behaviour { get; }

        ICoord[] OccupiedCoords { get; }

        int Width { get; }

        int Height { get; }

        bool RequiresLink { get; }

        Dictionary<ResourceType, int> GetYield(IStructure structure);

        void TurnEnd(IStructure structure);

        void TurnStart(IStructure structure);

        ICoord GetOrigin();

        string GetStatus();
    }
}