using Assets.Map;
using Assets.Resources;
using static Assets.Structures.StructureDelegates;

namespace Assets.Structures
{
    public interface IStructureFacade
    {
        string Address { get; }
        (ResourceType, int)[] Cost { get; }
        string Description { get; }
        string Name { get; }
        IStructureBehaviour BehaviorPrototype { get; }
        IStructurePlacementResult CanBePlacedInCell(Cell cell);
    }
}