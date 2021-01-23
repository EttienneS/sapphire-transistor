using Assets.Structures;
using Assets.Structures.Behaviors;

namespace Assets.MapGeneration
{
    public class NatureFacadeManager
    {
        public StructureFacade GetTree()
        {
            return new StructureFacade("Tree", 1, 1, "Tree", "Pine tree", _structureFactory.GetBehaviour<NoBehavior>());
        }

        public StructureFacade GetRock()
        {
            return new StructureFacade("Rock", 1, 1, "Rock", "A rock", _structureFactory.GetBehaviour<NoBehavior>());
        }

        private IStructureFactory _structureFactory;

        public NatureFacadeManager(IStructureFactory structureFactory)
        {
            _structureFactory = structureFactory;
        }
    }
}