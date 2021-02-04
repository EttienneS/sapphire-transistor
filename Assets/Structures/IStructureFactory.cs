using Assets.Map;

namespace Assets.Structures
{
    public interface IStructureFactory
    {
        IStructureBehaviour GetBehaviour<T>() where T : IStructureBehaviour;

        IStructure GetStructure(StructureType type, ICoord coord);
    }

    public enum StructureType
    {
        Tree, Rock, Core, Road, House, Barn, Field
    }
}