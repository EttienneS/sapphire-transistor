using Assets.Map;

namespace Assets.Structures
{
    public static class StructureDelegates
    {
        public delegate void StructurePlannedDelegate(IStructure structure);

        public delegate void StructureBuildProgressDelegate(IStructure structure, int current, int max);

        public delegate void StructureBuildCompletedDelegate(IStructure structure);

        public delegate void StructureDestroyedDelegate(IStructure structure);

        public delegate IStructurePlacementResult StructurePlacementValidationDelegate(Cell cell);
    }
}