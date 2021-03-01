using Assets.Map;

namespace Assets.Structures
{
    public interface IStructureFactory
    {

        IStructure GetStructure(StructureDefinition definition, ICoord coord);
        IStructureBehaviour GetBehaviour(string name);
    }
}