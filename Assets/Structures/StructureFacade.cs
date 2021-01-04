using Assets.Resources;

namespace Assets.Structures
{
    public class StructureFacade : IStructureFacade
    {
        public string Name { get; }

        public string Description { get; }

        public (ResourceType, int)[] Cost { get; }

        public IStructureBehaviour BehaviorPrototype { get; }

        public string Address { get; }

        public StructureFacade(string name, string assetName, string description, IStructureBehaviour structurePrototype) : this(name, assetName, description, structurePrototype, (ResourceType.Gold, 0))
        {
        }

        public StructureFacade(string name, string assetName, string description, IStructureBehaviour structurePrototype, params (ResourceType, int)[] cost)
        {
            Name = name;
            Address = assetName;
            Description = description;
            Cost = cost;
            BehaviorPrototype = structurePrototype;
        }
    }
}