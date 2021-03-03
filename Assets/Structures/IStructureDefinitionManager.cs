using Assets.Map;
using System.Collections.Generic;

namespace Assets.Structures
{

    public interface IStructureDefinitionManager
    {
        List<StructureDefinition> StructureDefinitions { get; }
        string GetAssetNameForStructureType(StructureDefinition.StructureType type);
        IPlacementResult CanPlace(Coord coord, StructureDefinition.StructureType type);
        StructureDefinition GetDefinitionForType(StructureDefinition.StructureType type);
    }
}