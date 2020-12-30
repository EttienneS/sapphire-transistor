using Assets.Map;
using Assets.Resources;

namespace Assets.Structures.Behaviors
{
    public class FarmBehavior : StructureBehaviorBase
    {
        private readonly (ResourceType, int)[] _baseYield;

        public FarmBehavior(IMapManager map) : base(map)
        {
            _baseYield = new[]
            {
                (ResourceType.Food, 1)
            };
        }

        public override (ResourceType, int)[] GetBaseYield(IStructure structure)
        {
            return _baseYield;
        }

        public override void TurnEnd(IStructure structure)
        {
        }

        public override void TurnStart(IStructure structure)
        {
        }
    }
}