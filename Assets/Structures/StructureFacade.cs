using Assets.Map;
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

        private StructureDelegates.StructurePlacementValidationDelegate _structurePlacementValidation;

        public StructureFacade(string name, string assetName, string description, IStructureBehaviour structurePrototype, StructureDelegates.StructurePlacementValidationDelegate structurePlacementValidationDelegate) : this(name, assetName, description, structurePrototype, structurePlacementValidationDelegate, (ResourceType.Gold, 0))
        {
        }

        public StructureFacade(string name, string assetName, string description, IStructureBehaviour structurePrototype, StructureDelegates.StructurePlacementValidationDelegate structurePlacementValidationDelegate, params (ResourceType, int)[] cost)
        {
            Name = name;
            Address = assetName;
            Description = description;
            Cost = cost;
            BehaviorPrototype = structurePrototype;
            _structurePlacementValidation = structurePlacementValidationDelegate;
        }

        public IStructurePlacementResult CanBePlacedInCell(Cell cell)
        {
            return _structurePlacementValidation.Invoke(cell);
        }
    }
}