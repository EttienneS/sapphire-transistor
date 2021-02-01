using Assets.Factions;
using Assets.Map;

namespace Assets.Structures.Behaviors
{
    public class HouseBehavior: StructureBehaviorBase
    {
        private readonly (ResourceType, int)[] _baseYield;

        public HouseBehavior(IMapManager map) : base(map)
        {
            _baseYield = new[]
            {
                (ResourceType.Gold, 1)
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