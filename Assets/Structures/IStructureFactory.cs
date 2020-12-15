using Assets.Map;

namespace Assets.Structures
{
    public interface IStructureFactory
    {
        IStructureBehaviour GetBehaviour<T>() where T : IStructureBehaviour;

        IStructure MakeStructure(IStructureFacade facade, ICoord coord);
    }
}