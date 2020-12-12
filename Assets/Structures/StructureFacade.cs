using Assets.Resources;

namespace Assets.Structures
{
    public class StructureFacade : IStructureFacade
    {
        public string Name { get; }

        public string Description { get; }

        public (ResourceType, int)[] Cost { get; }

        public IStructureBehaviour StructurePrototype { get; }

        public string AssetName { get; }

        public StructureFacade(string name, string assetName, string description, IStructureBehaviour structurePrototype, params (ResourceType, int)[] cost)
        {
            Name = name;
            AssetName = assetName;
            Description = description;
            Cost = cost;
            StructurePrototype = structurePrototype;
        }
    }
}