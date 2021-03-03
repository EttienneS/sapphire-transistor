using Assets.Map;

namespace Assets.Structures
{
    public interface IStructureFactory
    {

        IStructure GetStructure(StructureDefinition definition, Coord coord);
        IStructureBehaviour GetBehaviour(string name);
    }
}