using Assets.Map;
using Assets.Resources;

namespace Assets.Structures
{
    public abstract class StructureBehaviorBase : IStructureBehaviour
    {
        private readonly MapManager _map;

        public StructureBehaviorBase(MapManager map)
        {
            _map = map;
        }

        public abstract (ResourceType, int)[] GetYield(IStructure structure);

        public abstract void TurnEnd(IStructure structure);

        public abstract void TurnStart(IStructure structure);
    }
}