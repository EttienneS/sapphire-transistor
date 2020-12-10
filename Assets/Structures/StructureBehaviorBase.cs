using Assets.Map;
using Assets.Resources;

namespace Assets.Structures
{
    public abstract class StructureBehaviorBase : IStructureBehaviour
    {
        private readonly MapManager _map;

        public StructureBehaviorBase(MapManager map, IResourceManager resourceManager)
        {
            _map = map;
            ResourceManager = resourceManager;
        }

        public IResourceManager ResourceManager { get; }

        public abstract (IResource, int)[] GetYield(IStructure structure);

        public abstract void TurnEnd(IStructure structure);

        public abstract void TurnStart(IStructure structure);
    }
}