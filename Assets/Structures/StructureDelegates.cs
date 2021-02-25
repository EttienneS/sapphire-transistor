using Assets.Map;

namespace Assets.Structures
{
    public static class StructureDelegates
    {
        public delegate void StructurePlannedDelegate(IStructure structure);

        public delegate void StructureHighlightDelegate(IStructure structure);

        public delegate void StructureDestroyedDelegate(IStructure structure);

        public delegate IPlacementResult StructurePlacementValidationDelegate(Cell cell, int width, int height);
    }
}