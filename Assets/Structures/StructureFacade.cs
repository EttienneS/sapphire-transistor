using Assets.Resources;

namespace Assets.Structures
{
    public class StructureFacade : IStructureFacade
    {
        public string Name { get; }

        public string Description { get; }

        public (ResourceType, int)[] Cost { get; }

        public IStructureBehaviour StructurePrototype { get; }

        public StructureFacade(string name, string description, IStructureBehaviour structurePrototype, params (ResourceType, int)[] cost)
        {
            Name = name;
            Description = description;
            Cost = cost;
            StructurePrototype = structurePrototype;
        }
    }
}