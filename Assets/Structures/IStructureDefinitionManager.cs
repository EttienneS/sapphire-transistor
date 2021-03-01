using Assets.Map;
using System.Collections.Generic;

namespace Assets.Structures
{

    public interface IStructureDefinitionManager
    {
        List<StructureDefinition> StructureDefinitions { get; }
        string GetAssetNameForStructureType(StructureDefinition.StructureType type);
        IPlacementResult CanPlace(ICoord coord, StructureDefinition.StructureType type);
        StructureDefinition GetDefinitionForType(StructureDefinition.StructureType type);
    }
}