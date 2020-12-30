using Assets.Map;
using Assets.Resources;

namespace Assets.Structures
{
    public abstract class StructureBehaviorBase : IStructureBehaviour
    {
        private readonly IMapManager _map;

        public StructureBehaviorBase(IMapManager map)
        {
            _map = map;
        }

        public abstract (ResourceType, int)[] GetBaseYield(IStructure structure);

        public abstract void TurnEnd(IStructure structure);

        public abstract void TurnStart(IStructure structure);
    }
}