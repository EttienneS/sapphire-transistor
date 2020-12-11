using Assets.Resources;

namespace Assets.Structures
{
    public interface IStructureFacade
    {
        string Name { get; }

        string Description { get; }

        (ResourceType, int)[] Cost { get; }

        IStructureBehaviour StructurePrototype { get; }
    }
}