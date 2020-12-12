using Assets.Map;

namespace Assets.Structures
{
    public interface IStructure
    {
        IStructureBehaviour Behaviour { get; }
        ICoord Coord { get; }
        string Name { get; }
    }
}