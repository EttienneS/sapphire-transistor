using Assets.Map;
using Assets.Resources;

namespace Assets.Structures.Behaviors
{
    public class SettlementCore : StructureBehaviorBase
    {
        private readonly (ResourceType, int)[] _baseYield;


        public SettlementCore(MapManager map) : base(map)
        {
            _baseYield = new[]
            {
                (ResourceType.Gold, 1)
            };
        }

        public override (ResourceType, int)[] GetYield(IStructure structure)
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